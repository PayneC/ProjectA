local cf_ui = require('configs/cf_ui')
local cf_build = require('configs/cf_build')
local cf_item = require('configs/cf_item')

local m_item = require('models/m_item')
local m_build = require('models/m_build')

local c_build = require('controls/c_build')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')
local define = require('commons/define')
local event = define.event

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewItem(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')
	local sli_time_Slider = goUtil.GetComponent(gameObject, typeof(UnityEngine.UI.Slider), 'sli_time')
	
	local _item = {
		DID = 0,
		speed = 0,
		num = 0,
		count = 0,
		timePoint = 0,
		itemID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_num_TextEx = txt_num_TextEx,
		sli_time_Slider = sli_time_Slider,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			self.spr_icon_ImageEx:SetSprite('item', cf_item.GetData(itemID, cf_item.icon))
		else
			goUtil.SetActive(self.gameObject, false)
		end
		self:RefreshData()
	end
	
	function _item:RefreshData()
		local build = m_build.GetBuild(self.DID)
		if build.speed > 0 then
			self.speed = 3600 / build.speed
			debug.LogFormat(0, 'RefreshData self.speed = %d', self.speed)
		end
		self.num = build.num
		self.count = build.count
		self.timePoint = build.timePoint
		self.itemID = build.itemID
		
		local count = m_item.GetItemCount(self.itemID)
		self.txt_num_TextEx.text = string.format('%d/%d', count, self.count)
	end
	
	function _item:Update()
		if self.speed > 0 then
			local ct = time_mgr.GetTime()
			local num_add, num_m = math.modf((ct - self.timePoint) / self.speed)	
			self.sli_time_Slider.value = num_m		
		end
	end
	
	function _item:OnItemChange()
		local count = m_item.GetItemCount(self.itemID)
		self.txt_num_TextEx.text = string.format('%d/%d', count, self.count)
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
	local txt_item_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_name')
	local txt_item_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_num')
	
	local btn_uplv_ButtonEx = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'btn_uplv')
	local txt_uplv_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_uplv')
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_name_TextEx = txt_name_TextEx,
		txt_lv_TextEx = txt_lv_TextEx,
		spr_item_icon_ImageEx = spr_item_icon_ImageEx,
		txt_item_name_TextEx = txt_item_name_TextEx,
		txt_item_num_TextEx = txt_item_num_TextEx,
		btn_uplv_ButtonEx = btn_uplv_ButtonEx,
		txt_uplv_TextEx = txt_uplv_TextEx,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			self.txt_name_TextEx.text = cf_build.GetData(self.DID, cf_build.name)	
			self.spr_icon_ImageEx:SetSprite('build', cf_build.GetData(self.DID, cf_build.icon))
			self.txt_item_name_TextEx.text = cf_item.GetData(itemID, cf_item.name)	
			self.spr_item_icon_ImageEx:SetSprite('item', cf_item.GetData(itemID, cf_item.icon))
			
			local build = m_build.GetBuild(self.DID)
			if build then
				local lv = build.LV
				local speed = build.speed
				self.txt_lv_TextEx.text = string.format('LV.%d', lv)
				self.txt_item_num_TextEx.text = string.format('%d/小时', speed)
			end
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _item:OnClick()
		debug.Log(0, ' OnClick')
		c_build.BuildUpgrade(self.DID, 1)
	end
	
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
	events.AddListener(event.ItemChange, self.OnItemChange, self)
	events.AddListener(event.BuildChange, self.OnBuildChange, self)
	events.AddListener(event.BuildLVChange, self.OnBuildLVChange, self)
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'scr_items/Viewport/Content')

	self.rt_build = goUtil.FindChild(self.gameObject, 'rt_build')
	self.scr_builds_content = goUtil.FindChild(self.gameObject, 'scr_builds/Viewport/Content')
end

function _M:OnEnable()
	self:ShowBuilds()
	self:ShowItems()
end

function _M:Update(dt)
	local count = #self.items
	local item
	for i = 0, count, 1 do
		item = self.items[i]
		if item then
			item:Update()
		end
	end

	c_build.Calculate()
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowBuilds()
	local builds = m_build.GetAllBuild()
	local count = #builds
	local hasNum = #self.builds
	
	debug.LogFormat(0, 'count %d', count)
	debug.LogFormat(0, 'hasNum %d', hasNum)
	
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
		item:SetData(builds[i].DID)
	end
end


function _M:ShowItems()
	local builds = m_build.GetAllBuild()
	local count = #builds
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
		item:SetData(builds[i].DID)
	end
end

function _M:OnBuildChange()
	self:ShowBuilds()
	self:ShowItems()
end

function _M:OnBuildLVChange()
	self:ShowBuilds()
	self:ShowItems()
end

function _M:OnItemChange()
	self:ShowItems()
end

return _M 