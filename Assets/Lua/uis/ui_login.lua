local uibase = require('uis/ui_base')
local uimgr = require('base/ui_mgr')
local cf_ui = require('configs/cf_ui')
local goUtil = require('base/goutil')
local lvmgr = require('base/lv_mgr')

local prefs = require('base/prefs')
local c_data = require('controls/c_data')

local _M = class(uibase)

function _M:ctor()
	self.btn_start = nil
	self.btn_clear_ButtonEx = nil
	self.txt_start_TextEx = nil
end

function _M:OnLoaded()
	self.txt_start_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_start')
	
	self.btn_clear_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_clear')
	self.btn_clear_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClear, self))
	
	self.btn_start = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_start')
	self.btn_start.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnGameStart, self))
end

function _M:OnEnable()
	if prefs.GetTable('user') then
		self.txt_start_TextEx.text = '开始游戏'
	else
		self.txt_start_TextEx.text = '新建账号'
	end
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	self.btn_start.onClick:RemoveListener(UnityEngine.Events.UnityAction(self.OnGameStart, self))
end

function _M:OnGameStart()
	c_data.LoadData()
	lvmgr.LoadLevel(2, nil, true)
end

function _M:OnClear()
	prefs.Clear()
	if prefs.GetTable('user') then
		self.txt_start_TextEx.text = '开始游戏'
	else
		self.txt_start_TextEx.text = '新建账号'
	end
end

return _M 