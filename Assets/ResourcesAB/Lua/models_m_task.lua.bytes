local events = require('base/events')
local prefs = require('base/prefs')
local define = require('commons/define')
local eventType = require('commons/event_type')

local _tasks = {}

local _M = {}

local function NewTask()
	return {UID = 0, NPCID = 0, itemID = 0, rewards = {{10000003, 1000}, {10000002, 1000}}, CD = 0, timePoint = 0}
end

function _M.ReadData()
	_tasks = prefs.GetTable('_tasks') or {}
end

function _M.WriteData()
	if _isModify then
		prefs.SetTable('_tasks', _tasks)
		_isModify = false
	end
end

function _M.AddTask(UID)
	local task = NewTask()
	task.UID = UID
	table.insert(_tasks, task)
	return task
end

function _M.SetTaskDirty(UID)
	_isModify = true
	events.Brocast(eventType.TaskChange, UID)
end

function _M.GetTask(UID)
	local task
	for i = 1, #_tasks, 1 do
		task = _tasks[i]
		if task and task.UID == UID then
			return task
		end
	end
	return nil
end

function _M.GetAllTask()
	return _tasks
end

return _M 