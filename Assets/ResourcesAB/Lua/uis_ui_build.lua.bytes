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

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewItem(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')
	local spr_time_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_time')
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_num_TextEx = txt_num_TextEx,
		spr_time_ImageEx = spr_time_ImageEx,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			common.SetItemIcon(self.spr_icon_ImageEx, self.DID)			
		else
			goUtil.SetActive(self.gameObject, false)
		end
		self:RefreshData()
	end
	
	function _item:RefreshData()		
		local count = m_item.GetStuffCount(self.DID)
		local storage = m_item.GetStuffStorage(self.DID)
		self.txt_num_TextEx.text = string.format('%d', count)
		local p = 1
		if storage > 0 then
			p = count / storage
		end
		if p > 1 then
			p = 1
		end
		
		self.spr_time_ImageEx.fillAmount = p
	end
	
	function _item:OnItemChange()
		self:RefreshData()
	end
	
	goUtil.SetParent(_item.gameObject, _ui.scr_items_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	
	return _item
end

local function NewBuild(go)	
	local _item = {
		gameObject = go,
		UIGridElement = false,
		btn_build_bg_ButtonEx = false,
		btn_item_bg_ButtonEx = false,
		btn_get_ButtonEx = false,
		
		spr_build_ImageEx = false,
		spr_item_ImageEx = false,
		spr_item_p_ImageEx = false,
		
		txt_name_TextEx = false,
		txt_lv_TextEx = false,
		txt_num_TextEx = false,
		
		UID = 0,
		DID = 0,
		itemID = 0,
		tp = 0,
		needTime = 0,
		count = 0,
		itemCapacity = 0,
	}
	
	_item.UIGridElement = goUtil.GetComponent(_item.gameObject, typeof(UIGridElement), nil)
	
	_item.btn_build_bg_ButtonEx = goUtil.GetComponent(_item.gameObject, typeof(ButtonEx), 'btn_build_bg')
	_item.btn_item_bg_ButtonEx = goUtil.GetComponent(_item.gameObject, typeof(ButtonEx), 'btn_item_bg')
	_item.btn_get_ButtonEx = goUtil.GetComponent(_item.gameObject, typeof(ButtonEx), 'btn_get')
	
	_item.spr_build_ImageEx = goUtil.GetComponent(_item.gameObject, typeof(ImageEx), 'spr_build')
	_item.spr_item_ImageEx = goUtil.GetComponent(_item.gameObject, typeof(ImageEx), 'spr_item')
	_item.spr_item_p_ImageEx = goUtil.GetComponent(_item.gameObject, typeof(ImageEx), 'spr_item_p')
	
	_item.txt_name_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_name')
	_item.txt_lv_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_lv')
	_item.txt_num_TextEx = goUtil.GetComponent(_item.gameObject, typeof(TextEx), 'txt_num')
	
	function _item:OnIndexChange()
		self:SetData()
	end
	
	function _item:OnGet()
		c_build.CollectStuff(self.UID)
		--c_build.BuildUpgrade(self.UID)
	end
	
	function _item:OnBuildBg()
		uimgr.OpenSubUI(cf_ui.build_info, self.UID)
	end
	
	function _item:OnItemBg()
		
	end
	
	function _item:OnBuildChange(UID)
		if self.UID == UID then
			self:SetData()
		end
	end
	
	function _item:SetData()
		local index = _item.UIGridElement:GetIndex() + 1
		local build = m_build.GetBuildByIndex(index)
		
		if build then
			goUtil.SetActive(self.gameObject, true)
			self.UID = build.UID			
			self.DID = build.DID
			self.itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			
			self.tp = build.timePoint
			self.needTime = build.needTime
			
			self.count = build.count
			self.itemCapacity = build.itemCapacity
			
			local lv = cf_build.GetData(self.DID, cf_build.LV)
			local p = 1
			if self.count < self.itemCapacity then
				p = build.p
			end
			
			common.SetItemName(self.txt_name_TextEx, self.DID)
			common.SetItemIcon(self.spr_build_ImageEx, self.DID)
			common.SetItemIcon(self.spr_item_ImageEx, self.itemID)
			
			self.txt_lv_TextEx.text = string.format('等级 %d', lv)
			self.txt_num_TextEx.text = string.format('%d/%d', self.count, self.itemCapacity)
			self.spr_item_p_ImageEx.fillAmount = p
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _item:Update(ct)
		if self.count < self.itemCapacity then
			local p = 1
			if self.needTime > 0 then
				p =(self.needTime + self.tp - ct) / self.needTime
			end
			self.spr_item_p_ImageEx.fillAmount = 1 - p
		end
	end
	
	_item.btn_build_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnBuildBg, _item))
	_item.btn_item_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnItemBg, _item))
	_item.btn_get_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnGet, _item))
	
	_item.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_item.OnIndexChange, _item))
	
	return _item
end

function _M:ctor()
	self.builds = {}
	self.items = {}
	
	self.scr_builds_UIGrid = false
	
	self.rt_item = false
	self.scr_items_content = false
end

function _M:OnLoaded()
	self.scr_builds_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_builds')
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'rt_items')
	
	self.scr_builds_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))	
	
	events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	events.AddListener(eventType.BuildAdd, self.OnBuildAdd, self)
	events.AddListener(eventType.BuildChange, self.OnBuildChange, self)
end

function _M:OnEnable()
	self:ShowBuilds()
	self:ShowItems()
end

function _M:Update(dt)
	c_build.CalculateAll()
	local ct = time_mgr.GetTime()
	for i = 1, #self.builds, 1 do
		self.builds[i]:Update(ct)
	end
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:OnAddNewElement(go)
	local build = NewBuild(go)
	table.insert(self.builds, build)
end

function _M:ShowBuilds()
	local count = m_build.GetBuildCount()
	self.scr_builds_UIGrid:SetElementCount(count)
	self.scr_builds_UIGrid:ApplySetting()
end

function _M:ShowItems()
	local stuffs = m_item.GetAllStuff()
	--debug.LogFormat(0, debug.TableToString(stuffs))
	local count = #stuffs
	local hasNum = #self.items
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local item = NewItem(self)
			table.insert(self.items, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local item = self.items[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.items[i]
		item:SetData(stuffs[i].DID)
	end
end

function _M:OnBuildAdd()
	self:ShowBuilds()
end

function _M:OnBuildChange(UID)
	for i = 1, #self.builds, 1 do
		self.builds[i]:OnBuildChange(UID)
	end
end

function _M:OnItemChange()
	self:ShowItems()
end

return _M 