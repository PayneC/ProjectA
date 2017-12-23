local breakSocketHandle, debugXpCall = require("debug/LuaDebug")("localhost", 7003)

require('base/class')
local uimgr = require('base/ui_mgr')
local lvmgr = require('base/lv_mgr')

local debug = require('base/debug')
local events = require('base/events')

local a = {b = {c = 1, d = 2}, e = {3, 4, 5, 6}}

function OnEnter(parameter)
	lvmgr.Init()
	lvmgr.LoadLevel(1)
	
	debug.Log(0, debug.TableToString(a))
	
	local function ondsss()
		
	end
	events.AddListener(0, ondsss)
end

function Update(dt)
	uimgr.Update(dt)
	lvmgr.Update(dt)
end

function OnExit()
	uimgr.OnDestroy()
end 