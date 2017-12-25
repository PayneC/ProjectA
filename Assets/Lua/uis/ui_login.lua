local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')
local lvmgr = require('base/lv_mgr')

local _M = class(uibase)

function _M:ctor()
	self.btn_start = nil
end

function _M:OnLoaded()
	self.btn_start = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_start')
	self.btn_start.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnGameStart, self))
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	self.btn_start.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnGameStart, self))
end

function _M:OnGameStart()
	lvmgr.LoadLevel(3, nil, true)
end

return _M 