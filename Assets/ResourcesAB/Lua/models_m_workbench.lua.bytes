local events = require('base/events')
local prefsistence = require('base/prefsistence')
local define = require('commons/define')
local event = define.event

local _isModify = false
local _workbenchs

local _M = {}

local function NewWorkbench()
	
end

function _M.LoadData(data)
	_workbenchs = prefsistence.GetTable('workbench')
	if not _workbenchs then
		_workbenchs = {}
		for i = 1, 3, 1 do
			_workbenchs[i] = {UID = i, startTime = 0, formulaID = false}
		end
	end
end

function _M.SaveData()
	if _isModify then
		prefsistence.SetTable('workbench', _workbenchs)
		_isModify = false
	end
end

function _M.GetWorkbench(UID)
	for i = 1, #_workbenchs, 1 do
		local workbench = _workbenchs[i]
		if workbench and workbench.UID == UID then
			return workbench
		end
	end
end

function _M.SetWorkbench(UID, formulaID, startTime)
	for i = 1, #_workbenchs, 1 do
		local workbench = _workbenchs[i]
		if workbench and workbench.UID == UID then
			workbench.formulaID = formulaID
			workbench.startTime = startTime
			
			_isModify = true
			events.Brocast(event.WorkbenchChange)
		end
	end
end

function _M.GetAllWorkbench()
	return _workbenchs
end

return _M 