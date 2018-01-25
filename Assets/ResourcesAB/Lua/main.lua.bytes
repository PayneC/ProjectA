local breakSocketHandle, debugXpCall = require("debug/LuaDebug")("localhost", 7003)

debug = require('base/debug')
require('base/class')
require "event"

local uimgr = require('base/ui_mgr')
local lvmgr = require('base/lv_mgr')


local events = require('base/events')
local prefs = require('base/prefs')

local _controls = {
	require('controls/c_build'),
	require('controls/c_workbench'),
	require('controls/c_calculate'),
	require('controls/c_data')
}

local _controlCount = #_controls

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
	
	lvmgr.Init()
	lvmgr.LoadLevel(1)
end

function Update(dt)
	UpdateControl()
	
	uimgr.Update(dt)
	lvmgr.Update(dt)
	events.Update(dt)
	prefs.Update(dt)
end

function OnApplicationQuit()
	uimgr.OnDestroy()
end
