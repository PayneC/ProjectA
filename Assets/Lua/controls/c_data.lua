local prefs = require('base/prefs')

local _M = {}

local _models = {
	require('models/m_item'),
	require('models/m_build'),
	require('models/m_player'),
	require('models/m_workbench'),
	require('models/m_task'),
}

local _modelCount = #_models

local function WriteData()
	local model
	for i = 0, _modelCount, 1 do
		model = _models[i]
		if model and model.WriteData then
			model.WriteData()
		end
	end
end

local function ReadData()
	local model
	for i = 0, _modelCount, 1 do
		model = _models[i]
		if model and model.ReadData then
			model.ReadData()
		end
	end
end

local function NewData()
	local c_build = require('controls/c_build')
	local c_workbench = require('controls/c_workbench')
	local c_task = require('controls/c_task')
	local cf_init = require('configs/cf_init')
	for i = 1, #cf_init.unlockBuilds, 1 do
		c_build.NewBuild(cf_init.unlockBuilds[i])
	end
	
	for i = 1, cf_init.workbenchCount, 1 do
		c_workbench.NewWorkbench()
	end
	
	for i = 1, cf_init.taskCount, 1 do
		c_task.NewTask()
	end
	
	local cf_lv = require('csv/cf_lv')
	local m_player = require('models/m_player')
	local exp = cf_lv.GetData(1, cf_lv.exp)
	m_player.SetLv(1)
	m_player.SetExpMax(exp)
	
	prefs.SetTable('user', '1247286911111')
	WriteData()
end

function _M.LoadData()
	if prefs.GetTable('user') then
		ReadData()
	else
		NewData()
	end
	
	local m_trade = require('models/m_trade')
	m_trade.InitDataTemp()
end

function _M.Update(dt)
	WriteData()
end

return _M 