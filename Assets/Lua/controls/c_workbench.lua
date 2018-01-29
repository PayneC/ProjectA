local cf_formula = require('csv/cf_formula')

local m_workbench = require('models/m_workbench')
local m_item = require('models/m_item')
local m_formula = require('models/m_formula')

local time_mgr = require('base/time_mgr')

local common = require('misc/common')

local _M = {}

function _M.NewWorkbench()
	local UID = #m_workbench.GetAllWorkbench() + 1
	m_workbench.AddWorkbench(UID)
end

-- @UID:控制台ID
-- @DID：配方ID
function _M.MakeFormula(workbenchID, formulaID)
	debug.LogFormat(0, 'MakeFormula(%d, %d) time_mgr.GetTime()=%f', workbenchID, formulaID, time_mgr.GetTime())
	--check
	local stuffs = cf_formula.GetData(formulaID, cf_formula.stuff)
	local stuff
	for i = 1, #stuffs, 1 do
		stuff = stuffs[i]
		local need = stuff[2]
		local have = common.GetItemCount(stuff[1])
		if need > have then
			debug.LogFormat(0, 'MakeFormula 物品 %d 不够', stuff[1])
			return false
		end
	end
	
	for i = 1, #stuffs, 1 do
		stuff = stuffs[i]
		local need = stuff[2]
		common.CutItemCount(stuff[1], need)
	end
	
	m_workbench.SetWorkbench(workbenchID, formulaID, time_mgr.GetTime())
	return true
end

function _M.FinishFormula(workbenchID)
	debug.LogFormat(0, 'workbenchID(%d)', workbenchID)
	
	local workbench = m_workbench.GetWorkbench(workbenchID)
	local formulaID =(workbench and workbench.formulaID) or false
	local startTime =(workbench and workbench.startTime) or false
	if formulaID and startTime then
		
		local formula = m_formula.FindFormula(formulaID)
		if formula then
			local ct = time_mgr.GetTime()
			local timeCost = formula.timeCost or 0
			
			if ct >= startTime + timeCost then
				common.AddItemCount(formula.itemID, 1)
				m_workbench.SetWorkbench(workbenchID, false, false)
				
				formula.makeNum = formula.makeNum + 1
				if formula.makeNum == formula.nextNum then
					local rewards = cf_formula.GetData(formula.DID, cf_formula.reward)
					if rewards then
						--获取当前的生产次数达成奖励
						local reward = rewards[formula.rewardIndex]
						if reward then
							local funcID = reward[2] or 0
							local ItemID = reward[3] or 0
							local count = reward[4] or 0
							--To do
						end
						--获取下个奖励需要生产的次数
						formula.rewardIndex = formula.rewardIndex + 1
						local reward = rewards[formula.rewardIndex]
						if reward then
							formula.nextNum = reward[1] or 0
						end
					end
				end
				
				m_formula.SetDirtyFlag(formulaID)
			else
				-- 没完成
			end	
		end
		
	end
end

return _M 