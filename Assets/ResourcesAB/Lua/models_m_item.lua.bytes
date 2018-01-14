local events = require('base/events')
local prefs = require('base/prefs')
local eventType = require('commons/event_type')
local constant = require('commons/constant')

local debug = require('base/debug')

local _isModify = false

local _stuffs = {}
local _specialStuffs = {}
local _weapons = {}
local _weaponCount = 0
local _weaponStorage = 0

local _M = {}

local function NewStuff()
	local item = {DID = 0, count = 0, storage = 0}
	return item
end

local function NewWeapon()
	local item = {DID = 0, count = 0}
	return item
end

local function FindItem(DID, list)
	--debug.LogFormat(0, 'FindItem DID = %d, list = %s', DID, debug.TableToString(list or {}))
	if list and DID then
		local item
		for i = 1, #list, 1 do
			item = list[i]
			if item and item.DID == DID then
				return item
			end
		end
	end
	return nil
end

function _M.SetItemData(item, count, storage)
	if count then
		item.count = count
	end
	
	if item.storage and storage then
		item.storage = storage
	end
	
	_isModify = true
	events.Brocast(eventType.ItemChange)
end

function _M.GetItem(DID, newMiss)
	local type2 = math.modf(DID / 100000)
	local type1 = math.modf(type2 / 100)
	
	--debug.LogFormat(0, 'type1 = %d, ytpe2 = %d', type1, type2)
	local item
	if type1 == constant.Item_Stuff then
		if type2 == constant.Stuff_Normal then
			item = FindItem(DID, _stuffs)
			if not item and newMiss then
				item = NewStuff()
				item.DID = DID
				table.insert(_stuffs, item)
			end
		elseif type2 == constant.Stuff_Special then
			item = FindItem(DID, _specialStuffs)
			if not item and newMiss then
				item = NewStuff()
				item.DID = DID
				table.insert(_specialStuffs, item)
			end
		end
	elseif type1 == constant.Item_Weapon then
		item = FindItem(DID, _weapons)
		if not item and newMiss then
			item = NewWeapon()
			item.DID = DID
			table.insert(_weapons, item)
		end
	end
	return item, type1, type2
end

function _M.AddItemCount(DID, count)
	local item = _M.GetItem(DID, true)
	
	local newCount = item.count + count
	
	if type == 1 then
	elseif type == 2 then
		_weaponCount = _weaponCount + count
	end
	
	_M.SetItemData(item, newCount)
end

function _M.CutItemCount(DID, count)
	local item, type1 = _M.GetItem(DID, true)
	local cutCount = count
	if item.count < count then
		cutCount = item.count
	end
	
	local newCount = item.count - cutCount
	
	if type1 == 1 then
		
	elseif type1 == 2 then
		--还没有加判断
		_weaponCount = _weaponCount - cutCount
	end
	
	_M.SetItemData(item, newCount)
end

function _M.GetItemCount(DID)
	local item = _M.GetItem(DID, false)
	return item and item.count or 0
end

function _M.SetItemStorage(DID, storage)
	--debug.LogFormat(0, 'SetItemStorage(%d, %d)', DID, storage)
	local item, type1, type2 = _M.GetItem(DID, true)
	if item then
		_M.SetItemData(item, false, storage)
	else
		debug.LogErrorFormat('SetItemStorage Not Find Item %d type1 = %d type2 = %d', DID, type1, type2)
	end
end

function _M.GetItemStorage(DID)
	--debug.LogFormat(0, 'GetItemStorage(%d)', DID)
	local item = _M.GetItem(DID, false)
	return item and item.storage or 0
end

function _M.SetBagStorage(storage)
	_weaponStorage = storage or 0
end

function _M.GetBagStorage(storage)
	return _weaponStorage
end

function _M.GetAllWeapon()
	return _weapons
end

function _M.GetAllStuff()
	return _stuffs
end

function _M.GetAllSpecialStuff()
	return _specialStuffs
end

function _M.ReadData()
	_stuffs = prefs.GetTable('_stuffs') or {}
	_weapons = prefs.GetTable('_weapons') or {}
	_specialStuffs = prefs.GetTable('_specialStuffs') or {}
	_weaponCount = prefs.GetTable('_weaponCount') or 0
	_weaponStorage = prefs.GetTable('_weaponStorage') or {}
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_stuffs', _stuffs)
		prefs.SetTable('_weapons', _weapons)
		prefs.SetTable('_specialStuffs', _specialStuffs)
		prefs.SetTable('_weaponCount', _weaponCount)
		prefs.SetTable('_weaponStorage', _weaponStorage)
		_isModify = false
	end
end

return _M 