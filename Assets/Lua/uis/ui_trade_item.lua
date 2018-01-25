local cf_weapon = require('csv/cf_weapon')
local cf_item = require('csv/cf_item')
local cf_ui = require('configs/cf_ui')

local m_trade = require('models/m_trade')
local c_trade = require('controls/c_trade')

local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')

local uibase = require('uis/ui_base')

local _M = class(uibase)

function _M:ctor()
	self.btn_close_ButtonEx = false
	self.btn_coin_ButtonEx = false
	self.btn_cash_ButtonEx = false
	self.spr_icon_ImageEx = false
	self.txt_name_TextEx = false
	self.txt_bag_TextEx = false
	self.txt_trade_TextEx = false
	self.txt_coin_TextEx = false
	self.txt_cash_TextEx = false
	
	self.UID = 0
	self.DID = 0
end

function _M:OnLoaded()		
	self.btn_close_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	self.btn_coin_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_coin')
	self.btn_cash_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_cash')
	self.spr_icon_ImageEx = goUtil.GetComponent(self.gameObject, typeof(ImageEx), 'spr_icon')
	self.txt_name_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_name')
	self.txt_bag_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_bag')
	self.txt_trade_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_trade')
	self.txt_coin_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_coin')
	self.txt_cash_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_cash')
	
	self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnBtnClose, self))
	self.btn_coin_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnBtnCoin, self))
	self.btn_cash_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnBtnCash, self))
	
	events.AddListener(eventType.ItemChange, self.OnItemChange, self)
end

function _M:OnEnable(UID)
	self.UID = UID
	local sell = m_trade.GetSellByIndex(UID)
	if sell then
		self.DID = sell.DID
		common.SetItemIcon(self.spr_icon_ImageEx, sell.DID)
		common.SetItemName(self.txt_name_TextEx, sell.DID)
		
		self.txt_coin_TextEx.text = string.format('%d', sell.coin)
		self.txt_cash_TextEx.text = string.format('%d', sell.cash)
		
		local has = common.GetItemCount(sell.DID)
		self.txt_bag_TextEx.text = string.format('%d', has)
		
		self.txt_trade_TextEx.text = '∞'
	end
end

function _M:Refresh()
	local has = common.GetItemCount(self.DID)
	self.txt_bag_TextEx.text = string.format('%d', has)
end

function _M:OnBtnClose()
	uimgr.CloseUI(cf_ui.trade_item)
end

function _M:OnBtnCoin()
	c_trade.Buy(self.UID, false)
end

function _M:OnBtnCash()
	c_trade.Buy(self.UID, true)
end

function _M:OnItemChange()
	self:Refresh()
end

return _M 