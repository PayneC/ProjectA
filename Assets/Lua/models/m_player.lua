local events = require('base/events')
local prefs = require('base/prefs')
local eventType = require('misc/event_type')

local _isModify = false

local _cash = 0
local _coin = 0

local _exp = 0
local _expMax = 0
local _lv = 0

local _M = {}

function _M.SetExp(exp)
	if exp == _exp then
		return
	end
	
	_exp = exp or 0
	
	_isModify = true
	events.Brocast(eventType.EXPChange)
end

function _M.GetExp()
	return _exp or 0
end

function _M.SetExpMax(exp)
	if exp == _expMax then
		return
	end
	
	_expMax = exp or 0
	
	_isModify = true
	events.Brocast(eventType.EXPChange)
end

function _M.GetExpMax()
	return _expMax or 0
end

function _M.SetLv(lv)
	if _lv == lv then
		return
	end
	
	_lv = lv or 1
	
	_isModify = true
	events.Brocast(eventType.LVChange)
end

function _M.GetLv()
	return _lv or 1
end

function _M.SetCoin(coin)
	if _coin == coin then
		return
	end
	
	_coin = coin or 0
	
	_isModify = true
	events.Brocast(eventType.CoinChange)
end

function _M.GetCoin()
	return _coin or 0
end

function _M.SetCash(cash)
	if _cash == cash then
		return
	end
	
	_cash = cash or 0
	
	_isModify = true
	events.Brocast(eventType.CashChange)
end

function _M.GetCash()
	return _cash or 0
end

function _M.ReadData()
	_cash = prefs.GetTable('_cash') or 0
	_coin = prefs.GetTable('_coin') or 0
	_exp = prefs.GetTable('_exp') or 0
	_expMax = prefs.GetTable('_expMax') or 0
	_lv = prefs.GetTable('_lv') or 0
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_cash', _cash)
		prefs.SetTable('_coin', _coin)
		prefs.SetTable('_exp', _exp)
		prefs.SetTable('_expMax', _expMax)
		prefs.SetTable('_lv', _lv)
		_isModify = false
	end
end

return _M
