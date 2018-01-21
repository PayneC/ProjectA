local csv_weapon = require('csv/csv_weapon')

local m_task = require('models/m_task')
local m_item = require('models/m_item')

local time_mgr = require('base/time_mgr')

local _M = {}

local function RandomTask(task, cd)
	local weapons = csv_weapon.GetAllIndex()
	local index = math.random(#weapons)
	
	task.NPCID = 0
	task.itemID = weapons[index] or 0
	task.CD = cd
	task.timePoint = time_mgr.GetTime()
	m_task.SetTaskDirty(task.UID)
end

function _M.NewTask()
	local UID = #m_task.GetAllTask() + 1
	local task = m_task.AddTask(UID)
	RandomTask(task, 0)
end

function _M.FinishTask(UID)
	local task = m_task.GetTask(UID)
	local count = m_item.GetItemCount(task.itemID)
	if count > 0 then
		m_item.CutItemCount(task.itemID, 1)
		local reward
		for i = 1, #task.rewards, 1 do
			reward = task.rewards[i]
			if reward then
				m_item.AddItemCount(reward[1], reward[2])
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