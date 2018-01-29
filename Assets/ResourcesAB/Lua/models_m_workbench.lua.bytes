local events = require('base/events')
local prefs = require('base/prefs')

local eventType = require('misc/event_type')

local _isModify = false
local _workbenchs = {}

local _M = {}

local function NewWorkbench()
	return {UID = 0, formulaID = false, startTime = 0}
end

function _M.ReadData(data)
	_workbenchs = prefs.GetTable('_workbenchs')-- or {{UID = 1, formulaID = false, startTime = 0}, {UID = 2, formulaID = false, startTime = 0}, {UID = 3, formulaID = false, startTime = 0}}
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_workbenchs', _workbenchs)
		_isModify = false
	end
end

function _M.AddWorkbench(UID)
	local workbench = NewWorkbench()
	workbench.UID = UID
	table.insert(_workbenchs, workbench)
	
	_isModify = true
	events.Brocast(eventType.WorkbenchChange)
	return workbench
end

function _M.GetWorkbench(UID)
	for i = 1, #_workbenchs, 1 do
		local workbench = _workbenchs[i]
		if workbench and workbench.UID == UID then
			return workbench
		end
	end
end

function _M.GetEmptyBench()
	for i = 1, #_workbenchs, 1 do
		local workbench = _workbenchs[i]
		if workbench and not workbench.formulaID then
			return workbench.UID
		end
	end
	return false
end

function _M.SetWorkbench(UID, formulaID, startTime)
	for i = 1, #_workbenchs, 1 do
		local workbench = _workbenchs[i]
		if workbench and workbench.UID == UID then
			workbench.formulaID = formulaID
			workbench.startTime = startTime
			
			_isModify = true
			events.Brocast(eventType.WorkbenchChange)
		end
	end
end

function _M.GetAllWorkbench()
	return _workbenchs
end

return _M 