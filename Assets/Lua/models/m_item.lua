local events = require('base/events')
local prefs = require('base/prefs')
local eventType = require('misc/event_type')
local constant = require('misc/constant')

local _isModify = false

local _stuffs = {}
local _weapons = {}
local _weaponCount = 0
local _weaponStorage = 0

local _M = {}

local function _Find(DID, list)
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

local function _SetStuff(item, count, storage)
	if count then
		item.count = count
	end
	
	if item.storage and storage then
		item.storage = storage
	end
	
	_isModify = true
	events.Brocast(eventType.ItemChange)
end

local function _FindOrCreateStuff(DID)
	local item = _Find(DID, _stuffs)
	if not item then
		item = {DID = DID, count = 0, storage = 0}
		table.insert(_stuffs, item)
	end
	return item
end

function _M.FindStuff(DID)
	return _Find(DID, _stuffs)
end

function _M.AddStuffCount(DID, count)
	if count <= 0 then
		return
	end
	
	local item = _FindOrCreateStuff(DID)
	local newCount = item.count + count
	_SetStuff(item, newCount, false)
end

function _M.CutStuffCount(DID, count)
	if count <= 0 then
		return
	end
	
	local item = _M.FindStuff(DID)
	if item then
		local newCount = item.count - count
		if newCount < 0 then
			newCount = 0
		end
		_SetStuff(item, newCount, false)		
	end
end

function _M.GetStuffCount(DID)
	local item = _M.FindStuff(DID)
	return item and item.count or 0
end

function _M.SetStuffStorage(DID, storage)	
	local item = _FindOrCreateStuff(DID)
	_SetStuff(item, false, storage)
end

function _M.GetStuffStorage(DID)
	local item = _M.FindStuff(DID)
	return item and item.storage or 0
end

local function _SetWeapon(item, count)
	if item.count == count then
		return
	end
	
	_weaponCount = _weaponCount + count - item.count
	item.count = count
	
	_isModify = true
	events.Brocast(eventType.ItemChange)
end

local function _FindOrCreateWeapon(DID)
	local item = _Find(DID, _weapons)
	if not item then
		item = {DID = DID, count = 0}
		table.insert(_weapons, item)
	end
	return item
end

function _M.FindWeapon(DID)
	return _Find(DID, _weapons)
end

function _M.AddWeaponCount(DID, count)
	if count <= 0 then
		return
	end
	
	local item = _FindOrCreateWeapon(DID)
	_SetWeapon(item, item.count + count)
end

function _M.CutWeaponCount(DID, count)
	if count <= 0 then
		return
	end
	
	local item = _M.FindWeapon(DID)
	if not item then
		return	
	end
	
	local cutCount = count
	if cutCount > item.count then
		cutCount = item.count
	end
	
	_SetWeapon(item, item.count + cutCount)	
end

function _M.GetWeaponCount(DID)
	local item = _M.FindWeapon(DID)
	return item and item.count or 0
end

function _M.SetBagStorage(storage)
	_weaponStorage = storage or 0
	_isModify = true
	events.Brocast(eventType.ItemChange)
end

function _M.GetBagStorage()
	return _weaponStorage
end

function _M.GetBagCount()
	return _weaponCount
end

function _M.GetAllWeapon()
	return _weapons
end

function _M.GetAllStuff()
	return _stuffs
end

function _M.ReadData()
	_stuffs = prefs.GetTable('_stuffs') or {}
	_weapons = prefs.GetTable('_weapons') or {}
	_weaponCount = prefs.GetTable('_weaponCount') or 0
	_weaponStorage = prefs.GetTable('_weaponStorage') or 0
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_stuffs', _stuffs)
		prefs.SetTable('_weapons', _weapons)		
		prefs.SetTable('_weaponCount', _weaponCount)
		prefs.SetTable('_weaponStorage', _weaponStorage)
		_isModify = false
	end
end

return _M 