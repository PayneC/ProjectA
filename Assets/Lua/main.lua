local breakSocketHandle, debugXpCall = require("debug/LuaDebug")("localhost", 7003)

require('base/class')
require "event"

local uimgr = require('base/ui_mgr')
local lvmgr = require('base/lv_mgr')

local debug = require('base/debug')
local events = require('base/events')

function Main(parameter)
	UpdateBeat:Add(Update, self)
	
	lvmgr.Init()
	lvmgr.LoadLevel(1)
end

function Update(dt)
	uimgr.Update(dt)
	lvmgr.Update(dt)
end

function OnApplicationQuit()
	uimgr.OnDestroy()
end 