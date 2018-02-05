local cf_ui = require('configs/cf_ui')

local cf_workbench = require('csv/cf_workbench')

local m_workbench = require('models/m_workbench')
local m_player = require('models/m_player')

local c_workbench = require('controls/c_workbench')

local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')

local uibase = require('uis/ui_base')

local _M = class(uibase)

function _M:ctor()
	self.btn_bg_ButtonEx = false
	self.btn_close_ButtonEx = false
	
	self.btn_coin_ButtonEx = false
	self.btn_cash_ButtonEx = false
	
	self.txt_coin_TextEx = false
	self.txt_cash_TextEx = false
	
	self.txt_tip_TextEx = false
	self.txt_title_TextEx = false
end

function _M:OnLoaded()
	self.btn_bg_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bg')
	self.btn_close_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.btn_coin_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_coin')
	self.btn_cash_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_cash')
	
	self.txt_coin_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_coin')
	self.txt_cash_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_cash')
	
	self.txt_tip_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_tip')
	self.txt_title_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_title')
	
	events.AddListener(eventType.WorkbenchChangeAdd, self.OnWorkbenchChangeAdd, self)
	
	self.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
	self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
	self.btn_coin_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnCoin, self))
	self.btn_cash_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnCash, self))
end

function _M:OnEnable()
	self:Refresh()
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnClose()
	uimgr.CloseUI(cf_ui.workbench_add)
end

function _M:OnWorkbenchChangeAdd()
	self:Refresh()
end

function _M:OnCoin()
	c_workbench.BuyWorkbench(false)
end

function _M:OnCash()
	c_workbench.BuyWorkbench(true)
end

function _M:Refresh()
	local has = m_workbench.GetWorkbenchCount()
	local workbench = cf_workbench.GetDataEntire(has + 1)
	if workbench then
		local userLv = m_player.GetLv()
		if workbench[cf_workbench.lv] > userLv then
			self.txt_tip_TextEx.text = string.format('需要商店等级%d', workbench[cf_workbench.lv])
			self.btn_coin_ButtonEx.interactable = false
		else
			self.txt_tip_TextEx.text = '增加一个工作台'
			self.btn_coin_ButtonEx.interactable = true
		end
		
		self.txt_coin_TextEx.text = string.format('    %d', workbench[cf_workbench.cost])
		self.txt_cash_TextEx.text = string.format('    %d', workbench[cf_workbench.cost] * 0.1)
	else
		self.txt_tip_TextEx.text = '工作台已经到达购买上限'
	end
end

return _M 