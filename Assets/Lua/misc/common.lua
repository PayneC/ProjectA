local cf_item = require('csv/cf_item')
local cf_weapon = require('csv/cf_weapon')
local cf_build = require('csv/cf_build')
local cf_formula = require('csv/cf_formula')

local constant = require('misc/constant')

local _M = {}

function _M.GetItemType(itemID)
	if not itemID or itemID == 0 then
		return 0, 0
	end
	
	local type2 = math.modf(itemID / 100000)
	local type1 = math.modf(type2 / 100)
	
	return type1, type2
end

function _M.GetItemIcon(itemID)	
	local type1, type2 = _M.GetItemType(itemID)
	
	local atlas
	local icon
	
	if type1 == constant.Item_Stuff then
		atlas = 'item'
		icon = cf_item.GetData(itemID, cf_item.icon)
	elseif type1 == constant.Item_Weapon then
		atlas = 'item'
		icon = cf_weapon.GetData(itemID, cf_weapon.icon)
	elseif type1 == constant.Item_Build then
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
	local type1, type2 = _M.GetItemType(itemID)
	
	local name
	
	if type1 == constant.Item_Stuff then
		name = cf_item.GetData(itemID, cf_item.name)
	elseif type1 == constant.Item_Weapon then
		name = cf_weapon.GetData(itemID, cf_weapon.name)
	elseif type1 == constant.Item_Build then
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
	text.text = name
end

return _M 