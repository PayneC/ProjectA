local events = require('base/events')
local prefs = require('base/prefs')
local eventType = require('misc/event_type')

local _isModify = false

local _diamond = 0
local _gold = 0

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

function _M.SetGold(gold)
	if _gold == gold then
		return
	end
	
	_gold = gold or 0
	
	_isModify = true
	events.Brocast(eventType.GoldChange)
end

function _M.GetGold()
	return _gold or 0
end

function _M.SetDiamond(diamond)
	if _diamond == diamond then
		return
	end
	
	_diamond = diamond or 0
	
	_isModify = true
	events.Brocast(eventType.DiamondChange)
end

function _M.GetDiamond()
	return _diamond or 0
end

function _M.ReadData()
	_diamond = prefs.GetTable('_diamond') or 0
	_gold = prefs.GetTable('_gold') or 0
	_exp = prefs.GetTable('_exp') or 0
	_expMax = prefs.GetTable('_expMax') or 0
	_lv = prefs.GetTable('_lv') or 0
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_diamond', _diamond)
		prefs.SetTable('_gold', _gold)
		prefs.SetTable('_exp', _exp)
		prefs.SetTable('_expMax', _expMax)
		prefs.SetTable('_lv', _lv)
		_isModify = false
	end
end

return _M
