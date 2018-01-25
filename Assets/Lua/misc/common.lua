local cf_item = require('csv/cf_item')
local cf_weapon = require('csv/cf_weapon')
local cf_build = require('csv/cf_build')
local cf_formula = require('csv/cf_formula')

local m_item = require('models/m_item')
local m_player = require('models/m_player')

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
			if DID == constant.Item_Diamond then
				return m_player.GetDiamond()
			elseif DID == constant.Item_Glod then
				return m_player.GetGold()
			elseif DID == constant.Item_EXP then
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
	if count <= 0 then
		return
	end
	
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local realCount = count
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if DID == constant.Item_Diamond then
				m_player.SetDiamond(m_player.GetDiamond() + realCount)
			elseif DID == constant.Item_Glod then
				m_player.SetGold(m_player.GetGold() + realCount)
			elseif DID == constant.Item_EXP then
				m_player.SetExp(m_player.GetExp() + realCount)
			end
			
		elseif type2 == constant.Item_Stuff then
			m_item.AddStuffCount(itemID, realCount)
			
		elseif type2 == constant.Item_Weapon then
			m_item.AddWeaponCount(itemID, realCount)
			
		end
	end
end

function _M.CutItemCount(itemID, count)
	if count <= 0 then
		return
	end
	
	local type1, type2, type3 = _M.GetItemType(itemID)
	
	local realCount = count
	if type1 == constant.Item then
		if type2 == constant.Item_Special then
			if DID == constant.Item_Diamond then
				m_player.SetDiamond(m_player.GetDiamond() - realCount)
			elseif DID == constant.Item_Glod then
				m_player.SetGold(m_player.GetGold() - realCount)
			elseif DID == constant.Item_EXP then
				m_player.SetExp(m_player.GetExp() - realCount)
			end
			
		elseif type2 == constant.Item_Stuff then			
			m_item.CutStuffCount(itemID, realCount)
			
		elseif type2 == constant.Item_Weapon then
			m_item.CutWeaponCount(itemID, realCount)
			
		end
	end
end

return _M 