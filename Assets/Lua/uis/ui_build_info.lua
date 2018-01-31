local cf_ui = require('configs/cf_ui')
local cf_build = require('csv/cf_build')
local cf_item = require('csv/cf_item')

local m_item = require('models/m_item')
local m_build = require('models/m_build')

local c_build = require('controls/c_build')

local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')
local constant = require('misc/constant')

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewReward(go)	
	local _item = {
		gameObject = go,
		UIGridElement = false,
		
		txt_name_TextEx = false,
		txt_current_TextEx = false,
		txt_next_TextEx = false,
	}
	
	_item.UIGridElement = goUtil.GetComponent(_item.gameObject, typeof(UIGridElement), nil)
	
	_item.txt_name_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_name')
	_item.txt_current_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_current')
	_item.txt_next_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_next')
	
	function _item:OnIndexChange()
		
	end
	
	function _item:SetData(name, currentV, nextV)
		self.txt_name_TextEx.text = name
		self.txt_current_TextEx.text = string.format('%d', currentV)
		self.txt_next_TextEx.text = string.format('%d(+%d)', nextV, nextV - currentV)
	end
	
	return _item
end

function _M:ctor()
	self.btn_bg_ButtonEx = false
	self.btn_close_ButtonEx = false
	
	self.btn_coin_ButtonEx = false
	self.btn_cash_ButtonEx = false
	
	self.spr_icon_ImageEx = false
	
	self.txt_name_TextEx = false
	self.txt_lv_TextEx = false
	self.txt_coin_TextEx = false
	self.txt_cash_TextEx = false
	
	self.scr_rewards_UIGrid = false
	
	self.UID = 0
	self.rewards = {}
end

function _M:OnLoaded()	
	self.btn_bg_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bg')
	self.btn_close_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.btn_coin_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_coin')
	self.btn_cash_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_cash')
	
	self.spr_icon_ImageEx = goUtil.GetComponent(self.gameObject, typeof(ImageEx), 'spr_icon')
	
	self.txt_name_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_name')
	self.txt_lv_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_lv')
	self.txt_coin_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_coin')
	self.txt_cash_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_cash')
	
	self.scr_rewards_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_rewards')
	self.scr_rewards_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))	
	
	self.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
	self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
	
	self.btn_coin_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnCoin, self))
	self.btn_cash_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnCash, self))
	
	--events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	--events.AddListener(eventType.BuildAdd, self.OnBuildAdd, self)
	events.AddListener(eventType.BuildChange, self.OnBuildChange, self)
end

function _M:OnEnable(UID)
	self.UID = UID
	self:SetData()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnClose()
	uimgr.CloseUI(cf_ui.build_info)
end

function _M:OnCoin()
	c_build.BuildUpgrade(self.UID)
end

function _M:OnCash()
	c_build.BuildUpgrade(self.UID, true)
end

function _M:OnAddNewElement(go)
	local reward = NewReward(go)
	table.insert(self.rewards, reward)
end

function _M:OnBuildChange(UID)
	if self.UID == UID then
		self:SetData()
	end
end

function _M:SetData()
	local build = m_build.GetBuild(self.UID)
	local DID = build and build.DID
	common.SetItemIcon(self.spr_icon_ImageEx, DID)
	local lv = cf_build.GetData(DID, cf_build.LV)
	local name = cf_build.GetData(DID, cf_build.name)
	local costs = cf_build.GetData(DID, cf_build.unlockCost)
	local costCoin = 0
	for i = 1, #costs, 1 do
		local cost = costs[i]
		if cost then
			local costID = cost[1]
			local costCount = cost[2]
			if costID == constant.Item_Coin then
				costCoin = costCount or 0
			end
		end		
	end
	
	local costCash = common.GetItemCash(constant.Item_Coin, costCoin)
	self.txt_name_TextEx.text = name
	self.txt_lv_TextEx.text = string.format('%d', lv)
	self.txt_coin_TextEx.text = string.format('%d', costCoin)
	self.txt_cash_TextEx.text = string.format('%d', costCash)
	
	self.scr_rewards_UIGrid:SetElementCount(3)
	self.scr_rewards_UIGrid:ApplySetting()
	
	local nextLV = cf_build.GetData(DID, cf_build.nextLV)
	local curV = cf_build.GetData(DID, cf_build.itemCapacity)
	local nextV = cf_build.GetData(nextLV, cf_build.itemCapacity)
	self.rewards[3]:SetData('充值容量', curV, nextV)
	curV = cf_build.GetData(DID, cf_build.itemStorage)
	nextV = cf_build.GetData(nextLV, cf_build.itemStorage)
	self.rewards[2]:SetData('存储容量', curV, nextV)
	curV = cf_build.GetData(DID, cf_build.speed)
	nextV = cf_build.GetData(nextLV, cf_build.speed)
	self.rewards[1]:SetData('生产速度', curV, nextV)
end

return _M 