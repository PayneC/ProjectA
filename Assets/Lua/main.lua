local breakSocketHandle, debugXpCall = require("debug/LuaDebug")("localhost", 7003)

require('base/class')
require "event"

local uimgr = require('base/ui_mgr')
local lvmgr = require('base/lv_mgr')

local debug = require('base/debug')
local events = require('base/events')
local prefs = require('base/prefs')

local _models = {
	require('models/m_item'),
	require('models/m_build'),
	require('models/m_player'),
	require('models/m_workbench'),
	require('models/m_task'),
}

local _modelCount = #_models

local _controls = {
	require('controls/c_build'),
	require('controls/c_workbench'),
	require('controls/c_calculate'),
}

local _controlCount = #_controls

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
	
	prefs.SetTable('user', '1247286911111')
	WriteData()
end


local function UpdateControl()
	local control
	for i = 0, _controlCount, 1 do
		control = _controls[i]
		if control and control.Update then
			control.Update()
		end
	end
end

function Main(parameter)	
	math.randomseed(os.time())
	UpdateBeat:Add(Update, self)
	
	if prefs.GetTable('user') then
		ReadData()
	else
		NewData()
	end
	
	lvmgr.Init()
	lvmgr.LoadLevel(1)
end

function Update(dt)
	UpdateControl()
	WriteData()
	uimgr.Update(dt)
	lvmgr.Update(dt)
	events.Update(dt)
	prefs.Update(dt)
end

function OnApplicationQuit()
	uimgr.OnDestroy()
end
