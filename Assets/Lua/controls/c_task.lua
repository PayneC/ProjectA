local cf_weapon = require('csv/cf_weapon')

local m_task = require('models/m_task')
local m_item = require('models/m_item')
local m_formula = require('models/m_formula')

local time_mgr = require('base/time_mgr')

local common = require('misc/common')

local _M = {}

local function RandomTask(task, cd)
	local count = m_formula.GetFormulaCount()
	local index = math.random(count)
	
	local formula = m_formula.GetFormulaByIndex(index)
	if formula then
		task.NPCID = 0
		task.itemID = formula.itemID or 0
		task.CD = cd
		task.timePoint = time_mgr.GetTime()
		m_task.SetTaskDirty(task.UID)
	end
end

function _M.NewTask()
	local UID = #m_task.GetAllTask() + 1
	local task = m_task.AddTask(UID)
	RandomTask(task, 0)
end

function _M.FinishTask(UID)
	local task = m_task.GetTask(UID)
	local count = common.GetItemCount(task.itemID)
	if count > 0 then
		common.CutItemCount(task.itemID, 1)
		local reward
		for i = 1, #task.rewards, 1 do
			reward = task.rewards[i]
			if reward then
				common.AddItemCount(reward[1], reward[2])
			end
		end
		RandomTask(task, 5)
	end
end

function _M.CancelTask(UID)
	local task = m_task.GetTask(UID)
	RandomTask(task, 30)
end



return _M 