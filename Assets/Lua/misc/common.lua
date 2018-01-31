local cf_item = require('csv/cf_item')
local cf_weapon = require('csv/cf_weapon')
local cf_build = require('csv/cf_build')
local cf_formula = require('csv/cf_formula')
local cf_lv = require('csv/cf_lv')

local m_item = require('models/m_item')
local m_player = require('models/m_player')
local m_tip = require('models/m_tip')

local constant = require('misc/constant')

local _M = {}

function _M.GetItemType(itemID)
	if not itemID or itemID == 0 then
		return 0, 0, 0
	end
	
	local type1 = math.modf(itemID / 10000000)
	local type2 = math.modf(itemID / 1000000)
	local type3 = math.modf((itemID - type2 * 1000000) / 10000)
	
	return type1, type2, type3
end

function _M.GetItemPrice(itemID)
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local coin, cash
	
	if type1 == constant.Item then
		if type2 == constant.Item_Special or type2 == constant.Item_Stuff then
			coin = cf_item.GetData(itemID, cf_item.coin)
			cash = cf_item.GetData(itemID, cf_item.cash)
		elseif type2 == constant.Item_Weapon then
			coin = cf_weapon.GetData(itemID, cf_weapon.coin)
			cash = cf_weapon.GetData(itemID, cf_weapon.cash)
		end
	end
	
	return coin, cash
end

function _M.GetItemIcon(itemID)	
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local atlas
	local icon
	
	if type1 == constant.Item then
		if type2 == constant.Item_Special or type2 == constant.Item_Stuff then
			atlas = 'item'
			icon = cf_item.GetData(itemID, cf_item.icon)
		elseif type2 == constant.Item_Weapon then
			atlas = 'item'
			icon = cf_weapon.GetData(itemID, cf_weapon.icon)
		end
	elseif type1 == constant.Build then
		atlas = 'build'
		icon = cf_build.GetData(itemID, cf_build.icon)		
	elseif type1 == constant.Formula then
		local DID = cf_formula.GetData(itemID, cf_formula.itemID)
		local a, i = _M.GetItemIcon(DID)
		atlas = a
		icon = i
	end
	
	if not icon then
		atlas = 'item'
		icon = 'item_0'
	end
	
	return atlas, icon
end

function _M.GetItemName(itemID)
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local name
	
	if type1 == constant.Item then
		if type2 == constant.Item_Special or type2 == constant.Item_Stuff then
			name = cf_item.GetData(itemID, cf_item.name)
		elseif type2 == constant.Item_Weapon then
			name = cf_weapon.GetData(itemID, cf_weapon.name)
		end
	elseif type1 == constant.Build then
		name = cf_build.GetData(itemID, cf_build.name)
	end
	
	return name
end

function _M.SetItemIcon(sprite, itemID)
	if not sprite then
		return
	end
	local atlas, icon = _M.GetItemIcon(itemID)	
	sprite:SetSprite(atlas, icon)
end

function _M.SetItemName(text, itemID)
	if not text then
		return
	end	
	local name = _M.GetItemName(itemID)	
	text.text = name or ''
end

function _M.GetItemCount(itemID)
	local type1, type2, type3 = _M.GetItemType(itemID)
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if itemID == constant.Item_Cash then
				return m_player.GetCash()
			elseif itemID == constant.Item_Coin then
				return m_player.GetCoin()
			elseif itemID == constant.Item_EXP then
				return m_player.GetExp()
			end
		elseif type2 == constant.Item_Stuff then
			return m_item.GetStuffCount(itemID)
		elseif type2 == constant.Item_Weapon then
			return m_item.GetWeaponCount(itemID)
		end
	end
	return 0
end

function _M.AddItemCount(itemID, count)
	debug.LogFormat(0, 'AddItemCount(%d, %d)', itemID, count)
	if count <= 0 then
		return
	end
	
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local realCount = count
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if itemID == constant.Item_Cash then
				m_player.SetCash(m_player.GetCash() + realCount)
			elseif itemID == constant.Item_Coin then
				m_player.SetCoin(m_player.GetCoin() + realCount)
			elseif itemID == constant.Item_EXP then
				local exp = m_player.GetExp() + realCount
				local expMax = m_player.GetExpMax()
				if expMax > 0 then
					while exp >= expMax do
						exp = exp - expMax
						_M.LvUp()
					end
					m_player.SetExp(exp)
				end
			end
			
		elseif type2 == constant.Item_Stuff then
			m_item.AddStuffCount(itemID, realCount)
			
		elseif type2 == constant.Item_Weapon then
			m_item.AddWeaponCount(itemID, realCount)
			
		end
	end
	
	m_tip.PushEntity(itemID, realCount)
end

function _M.CutItemCount(itemID, count)
	if count <= 0 then
		return
	end
	
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local realCount = count
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if itemID == constant.Item_Cash then
				m_player.SetCash(m_player.GetCash() - realCount)
			elseif itemID == constant.Item_Coin then
				m_player.SetCoin(m_player.GetCoin() - realCount)
			elseif itemID == constant.Item_EXP then
				m_player.SetExp(m_player.GetExp() - realCount)
			end
			
		elseif type2 == constant.Item_Stuff then			
			m_item.CutStuffCount(itemID, realCount)
			
		elseif type2 == constant.Item_Weapon then
			m_item.CutWeaponCount(itemID, realCount)
			
		end
	end
end

function _M.CheckCosts(costs, returnLack)
	if not costs then
		return true
	end
	
	local cost, itemID, count
	for i = 1, #costs, 1 do
		cost = costs[i]
		itemID = cost and cost[1] or false
		count = cost and cost[2] or 0
		
		if itemID and _M.GetItemCount(itemID) < count then
			return false
		end
	end
	
	return true
end

function _M.GetCostsLack()
	if not costs then
		return nil
	end
	
	local lack
	local cost, itemID, count
	for i = 1, #costs, 1 do
		cost = costs[i]
		itemID = cost and cost[1] or false
		count = cost and cost[2] or 0
		
		if itemID then
			local has = _M.GetItemCount(itemID)
			if has < count then
				if not lack then
					lack = {}
				end
				
				table.insert(lack, {itemID, count - has})
			end
		end
	end
	
	return lack
end

function _M.LvUp()
	m_player.SetLv(m_player.GetLv() + 1)
	local exp = cf_lv.GetData(m_player.GetLv(), cf_lv.exp)
	m_player.SetExpMax(exp)
	
	local reward
	local rewards = cf_lv.GetData(m_player.GetLv(), cf_lv.reward)
	for i = 1, #rewards, 1 do
		reward = rewards[i]
		if reward then
			_M.AddItemCount(reward[1], reward[2])
		end
	end
end

function _M.CashBuy(DID, count)
	local coin, cash = _M.GetItemPrice(DID)
	local has = _M.GetItemCount(constant.Item_Cash)
	cash = cash or 0
	if cash * count > has then
		return
	end
	
	_M.AddItemCount(DID, count)
	if cash * count > 0 then
		_M.CutItemCount(constant.Item_Cash, cash * count)
	end
end

function _M.GetItemCash(itemID, count)
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local cash = 0
	
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if itemID == constant.Item_Cash then
				cash = count
			elseif itemID == constant.Item_Coin then
				cash = count * 0.1
			elseif itemID == constant.Item_EXP then
				cash = 0
			end			
		elseif type2 == constant.Item_Stuff then			
			cash = 0
		elseif type2 == constant.Item_Weapon then
			cash = cf_weapon.GetData(itemID, cf_weapon.priceZS)
		end
	end
	
	return cash
end

return _M 