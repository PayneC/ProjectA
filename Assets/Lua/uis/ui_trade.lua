local cf_weapon = require('csv/cf_weapon')
local cf_item = require('csv/cf_item')
local cf_ui = require('configs/cf_ui')

local m_trade = require('models/m_trade')

local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')

local common = require('misc/common')

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewItem(go)
	local _item = {
		gameObject = go,
		spr_bg_ButtonEx = false,
		UIGridElement = false,
		spr_icon_ImageEx = false,
		txt_name_TextEx = false,
		txt_bag_TextEx = false,
		txt_trade_TextEx = false,
		txt_gold_TextEx = false,
		txt_cash_TextEx = false,
		
		UID = 0,
	}
	
	_item.spr_bg_ButtonEx = goUtil.GetComponent(_item.gameObject, typeof(ButtonEx), 'spr_bg')
	_item.UIGridElement = goUtil.GetComponent(_item.gameObject, typeof(UIGridElement), nil)
	_item.spr_icon_ImageEx = goUtil.GetComponent(_item.gameObject, typeof(ImageEx), 'spr_icon')
	_item.txt_name_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_name')
	_item.txt_bag_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_bag')
	_item.txt_trade_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_trade')
	_item.txt_gold_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_gold')
	_item.txt_cash_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_cash')
	
	function _item:OnIndexChange()
		self:RefreshData()
	end
	
	function _item:RefreshData()		
		local index = self.UIGridElement:GetIndex() + 1
		local sell = m_trade.GetSellByIndex(index)
		if sell then
			self.UID = sell.UID
			common.SetItemIcon(self.spr_icon_ImageEx, sell.DID)
			common.SetItemName(self.txt_name_TextEx, sell.DID)
			
			self.txt_gold_TextEx.text = string.format('%d', sell.coin)
			self.txt_cash_TextEx.text = string.format('%d', sell.cash)
			
			local has = common.GetItemCount(sell.DID)
			self.txt_bag_TextEx.text = string.format('%d', has)
			
			self.txt_trade_TextEx.text = '∞'
		end
	end
	
	function _item:OnClick()
		uimgr.OpenSubUI(cf_ui.trade_item, self.UID)
	end
	
	_item.spr_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnClick, _item))
	_item.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_item.OnIndexChange, _item))
	
	return _item
end

function _M:ctor()
	self.scr_items_UIGrid = false
	
	self.items = {}
end

function _M:OnLoaded()		
	self.scr_items_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_items')
	self.scr_items_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))
end

function _M:OnEnable()
	self.scr_items_UIGrid:SetElementCount(m_trade.GetSellCount())
	self.scr_items_UIGrid:ApplySetting()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnBuildChange()
	
end

function _M:OnBuildLVChange()
	
end

function _M:OnItemChange()
	
end

function _M:OnAddNewElement(go)
	local item = NewItem(go)
	table.insert(self.items, item)
end

return _M 