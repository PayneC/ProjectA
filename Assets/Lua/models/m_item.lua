local events = require('base/events')
local prefsistence = require('base/prefsistence')
local define = require('commons/define')
local event = define.event

local _isModify = false
local _items

local _M = {}

local function NewItem()
	local item = {DID = 0, count = 0, limit = 0}
	return item
end

function _M.AddItemCount(DID, count)
	local item = _items[DID]
	if not item then
		item = NewItem()
		item.DID = DID
		_items[DID] = item
	end
	item.count = item.count + count
	
	_isModify = true
	events.Brocast(event.ItemChange)
end

function _M.SetItemCount(DID, count)
	local item = _items[DID]
	if not item then
		item = NewItem()
		item.DID = DID
		_items[DID] = item
	end
	item.count = count
	
	_isModify = true
	events.Brocast(event.ItemChange)
end

function _M.GetItemCount(DID)
	local item = _items[DID]
	if item then
		return item.count
	end
	return 0
end

function _M.SetItemLimit(DID, limit)
	local item = _items[DID]
	if not item then
		item = {DID = DID, count = 0, limit = limit}
	end
	item.limit = limit
	
	_isModify = true
	events.Brocast(event.ItemChange)
end

function _M.GetItemLimit(DID)
	local item = _items[DID]
	if item then
		return item.limit
	end
	return 0
end

function _M.GetAllItem()
	return _items
end

function _M.LoadData(data)
	_items = prefsistence.GetTable('item') or {}
end

function _M.SaveData()
	if _isModify then
		prefsistence.SetTable('item', _items)
		_isModify = false
	end
end

return _M 