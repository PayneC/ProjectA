local cf_weapon = require('csv/cf_weapon')

local events = require('base/events')
local eventType = require('misc/event_type')
local prefs = require('base/prefs')

local time_mgr = require('base/time_mgr')

local _isModify = false

local _sells = {}
local _buys = {}

local _M = {}

local function NewItem()
	local _item = {
		UID = 0,
		DID = 0,
		coin = 0,
		cash = 0,
	}
	return _item
end

function _M.InitDataTemp()
	local ids = cf_weapon.GetAllIndex()
	for i = 1, #ids, 1 do
		local id = ids[i]
		local item = NewItem()
		table.insert(_sells, item)
		item.UID = i
		item.DID = id
		item.coin = cf_weapon.GetData(id, cf_weapon.coin)
		item.cash = cf_weapon.GetData(id, cf_weapon.cash)
	end
end

function _M.GetSellCount()
	return #_sells
end

function _M.GetSellByIndex(index)
	return _sells[index]
end

function _M.GetSellByID(UID)
	return _sells[UID]
end

function _M.ReadData()
	
end

function _M.WriteData()
	
end

return _M 