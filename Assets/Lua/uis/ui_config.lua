local cf_ui = require('configs/cf_ui')
local cf_build = require('configs/cf_build')

local m_build = require('models/m_build')

local debug = require('base/debug')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')
local define = require('commons/define')
local event = define.event

local uibase = require('uis/ui_base')

local _M = class(uibase)

function _M:ctor()
	self.txt_info = false
	
	self.OnBuildChange = nil
end

function _M:OnLoaded()
	self.txt_info = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_info')
	
	--self.txt_info.text = 'payne'
	
	
	local function OnBuildChange()
		self:ShowBuilds()
	end
	
	self.OnBuildChange = OnBuildChange
	events.AddListener(event.BuildChange, OnBuildChange)
end

function _M:OnEnable()
	self:ShowBuilds()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowBuilds()
	local builds = m_build.GetBuilds()
	
	local count = #builds
	
	local s = 'payne'
	for i = 1, count, 1 do
		local DID = builds[i]
		string.format('%s\n%s', s, cf_build.GetData(DID, cf_build.name))
    end
    debug.Log(0, ' ShowBuilds')
    debug.Log(0, debug.TableToString(builds))


	self.txt_info.text = s
end

return _M 