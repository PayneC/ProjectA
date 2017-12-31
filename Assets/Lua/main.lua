local breakSocketHandle, debugXpCall = require("debug/LuaDebug")("localhost", 7003)

require('base/class')
require "event"

local uimgr = require('base/ui_mgr')
local lvmgr = require('base/lv_mgr')

local debug = require('base/debug')
local events = require('base/events')

local _models = {
	require('models/m_assets'),
	require('models/m_item'),
	require('models/m_player'),
	require('models/m_workbench'),
}

local _modelCount = #_models

local _controls = {
	require('controls/c_calculate'),
	require('controls/c_player'),
}

local _controlCount = #_controls

function Main(parameter)
	UpdateBeat:Add(Update, self)
	
	lvmgr.Init()
	lvmgr.LoadLevel(1)
end

function Update(dt)
	local control
	for i = 0, _controlCount, 1 do
		control = _controls[i]
		if control and control.Update then
			control.Update()
		end
	end
	
	uimgr.Update(dt)
	lvmgr.Update(dt)
end

function OnApplicationQuit()
	uimgr.OnDestroy()
end 