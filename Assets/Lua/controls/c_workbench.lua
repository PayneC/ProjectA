local csv_formula = require('csv/csv_formula')

local m_workbench = require('models/m_workbench')
local m_item = require('models/m_item')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')

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
	local stuffs = csv_formula.GetData(formulaID, csv_formula.stuff)
	local stuff
	for i = 1, #stuffs, 1 do
		stuff = stuffs[i]
		local need = stuff[2]
		local have = m_item.GetItemCount(stuff[1])
		if need > have then
			debug.LogFormat(0, 'MakeFormula 物品 %d 不够', stuff[1])
			return false
		end
	end
	
	for i = 1, #stuffs, 1 do
		stuff = stuffs[i]
		local need = stuff[2]
		m_item.CutItemCount(stuff[1], need)
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
		local ct = time_mgr.GetTime()
		local timeCost = csv_formula.GetData(formulaID, csv_formula.timeCost)
		if ct >= startTime + timeCost then
			local itemID = csv_formula.GetData(formulaID, csv_formula.itemID)
			
			m_item.AddItemCount(itemID, 1)
			m_workbench.SetWorkbench(workbenchID, false, false)
		else
			-- 没完成
		end	
	end
end

return _M 