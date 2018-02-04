-- 记录资源建筑相关的数据
-- 资源的状态 未解锁 解锁
-- 资源的属性 等级
local cf_formula = require('csv/cf_formula')

local events = require('base/events')
local eventType = require('misc/event_type')
local prefs = require('base/prefs')

local time_mgr = require('base/time_mgr')

local _isModify = false
local _formulas = {}
local _itemCorrelationFormula = {}

local _M = {}

local function NewFormula()
	local _formula = {
		DID = 0,
		itemID = 0,
		makeNum = 0,
		nextNum = 0,
		rewardIndex = 1,
		timeCost = 0,
		overrideStuff = false,
	}
	return _formula
end

function _M.FindFormula(DID)
	local formula
	for i = 1, #_formulas, 1 do
		formula = _formulas[i]
		if formula.DID == DID then
			return formula
		end
	end
end

function _M.AddFormula(DID)
	local formula = _M.FindFormula(DID)
	if not formula then	
		formula = NewFormula()
		formula.DID = DID
		formula.itemID = cf_formula.GetData(formula.DID, cf_formula.itemID)
		formula.makeNum = 0
		formula.timeCost = cf_formula.GetData(formula.DID, cf_formula.timeCost)
		local rewards = cf_formula.GetData(formula.DID, cf_formula.reward)
		if rewards then
			local reward = rewards[1]
			formula.nextNum = reward and reward[1] or 0
		end
		table.insert(_formulas, formula)
		_isModify = true
		
		events.Brocast(eventType.FormulaAdd)
	end
	return formula
end

function _M.SetDirtyFlag(DID)
	_isModify = true
	events.Brocast(eventType.FormulaChange, DID)
end

function _M.GetFormulaByIndex(index)
	if _formulas then
		return _formulas[index]
	else
		return nil
	end
end

function _M.GetAllFormula()
	return _formulas
end

function _M.GetFormulaCount()
	if _formulas then
		return #_formulas
	else
		return 0
	end
end

function _M.ReadData()
	_formulas = prefs.GetTable('_formulas') or {}
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_formulas', _formulas)
		_isModify = false
	end
end

function _M.ParseData()
	_M.ParseItemCorrelationFormula()
end

function _M.GetItemCorrelationFormula(itemID)
	return _itemCorrelationFormula[itemID]
end

function _M.ParseItemCorrelationFormula()
	for i = 1, #_formulas, 1 do
		local formula = _formulas[i]
		if formula then
			_itemCorrelationFormula[formula.itemID] = formula
		end
	end
end

return _M 