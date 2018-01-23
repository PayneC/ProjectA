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

local function NewBuild(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_build)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_name')
	local txt_lv_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_lv')
	
	local spr_item_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_item_icon')
	local spr_item_p_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_item_p')
	local txt_item_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_num')
	
	local spr_item_icon_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'spr_item_icon')
	local btn_uplv_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'btn_uplv')
	local txt_uplv_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_uplv')
	
	local _item = {
		UID = 0,
		DID = 0,
		tp = 0,
		needTime = 0,
		count = 0,
		itemCapacity = 0,
		
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_name_TextEx = txt_name_TextEx,
		txt_lv_TextEx = txt_lv_TextEx,
		spr_item_icon_ImageEx = spr_item_icon_ImageEx,
		spr_item_p_ImageEx = spr_item_p_ImageEx,
		txt_item_num_TextEx = txt_item_num_TextEx,
		spr_item_icon_ButtonEx = spr_item_icon_ButtonEx,
		btn_uplv_ButtonEx = btn_uplv_ButtonEx,
		txt_uplv_TextEx = txt_uplv_TextEx,
	}
	
	function _item:SetData(UID)
		self.UID = UID	
		local build = m_build.GetBuild(self.UID)
		if self.UID and build then
			self.DID = build.DID
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			
			common.SetItemName(self.txt_name_TextEx, self.DID)
			common.SetItemIcon(self.spr_icon_ImageEx, self.DID)
			
			common.SetItemIcon(self.spr_item_icon_ImageEx, itemID)
			common.SetItemIcon(self.spr_item_p_ImageEx, itemID)
			
			local lv = cf_build.GetData(self.DID, cf_build.LV)			
			self.txt_lv_TextEx.text = string.format('LV.%d', lv)
			self.txt_item_num_TextEx.text = string.format('%d', build.count)
			
			
			
			self.tp = build.timePoint
			self.needTime = build.needTime
			
			self.count = build.count
			self.itemCapacity = build.itemCapacity
			
			if self.count >= self.itemCapacity then
				self.spr_item_p_ImageEx.fillAmount = 1
			else
				self.spr_item_p_ImageEx.fillAmount = build.p
				
			end
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _item:OnClick()
		debug.Log(0, ' OnClick')
		c_build.BuildUpgrade(self.UID)
	end
	
	function _item:OnCollect()
		c_build.CollectStuff(self.UID)
	end
	
	function _item:Update(ct)
		if self.count >= self.itemCapacity then
			return
		end
		local p = 1
		if self.needTime > 0 then
			p =(self.needTime + self.tp - ct) / self.needTime
		end
		self.spr_item_p_ImageEx.fillAmount = 1 - p
	end
	
	_item.spr_item_icon_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnCollect, _item))
	
	_item.btn_uplv_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnClick, _item))
	goUtil.SetParent(_item.gameObject, _ui.scr_builds_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	
	return _item
end

function _M:ctor()
	self.builds = {}
	self.items = {}
end

function _M:OnLoaded()
	events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	events.AddListener(eventType.BuildChange, self.OnBuildChange, self)
	events.AddListener(eventType.BuildLVChange, self.OnBuildLVChange, self)
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'rt_items')
	
	self.rt_build = goUtil.FindChild(self.gameObject, 'rt_build')
	self.scr_builds_content = goUtil.FindChild(self.gameObject, 'scr_builds/Viewport/Content')
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

function _M:ShowBuilds()
	local builds = m_build.GetAllBuild()
	local count = #builds
	local hasNum = #self.builds
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local item = NewBuild(self)
			table.insert(self.builds, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local item = self.builds[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.builds[i]
		item:SetData(builds[i].UID)
	end
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

function _M:OnBuildChange()
	self:ShowBuilds()
end

function _M:OnBuildLVChange()
	self:ShowBuilds()
end

function _M:OnItemChange()
	self:ShowItems()
end

return _M 