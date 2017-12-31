local cf_ui = require('configs/cf_ui')
local cf_build = require('configs/cf_build')
local cf_item = require('configs/cf_item')

local m_build = require('models/m_build')

local debug = require('base/debug')
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
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_num_TextEx = txt_num_TextEx,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			self.spr_icon_ImageEx:SetSprite('uimain', cf_build.GetData(self.DID, cf_build.icon))
		else
			goUtil.SetActive(self.gameObject, false)
		end
		self:RefreshData()
	end
	
	function _item:RefreshData()
		
	end
	
	goUtil.SetParent(_item.gameObject, _ui.scr_items_content)
	
	return _item
end

local function NewBuild(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_build)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_state_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_state')
	local btn_work = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'btn_work')
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_state_TextEx = txt_state_TextEx,
		btn_work = btn_work,
	}
	
	function _item:SetData(DID, startTime)
		self.DID = DID	
		if self.DID then
			goUtil.SetActiveByComponent(self.spr_icon_ImageEx, true)
			self.txt_state_TextEx.text = '添加'
			local icon = cf_item.GetData(self.DID, cf_item.icon)
			self.spr_item_icon_ImageEx:SetSprite('uimain', icon)
		else
			goUtil.SetActiveByComponent(self.spr_icon_ImageEx, false)
			self.txt_state_TextEx.text = ''
		end
	end

	function _item:SetActive(active)
		goUtil.SetActive(self.gameObject, active)
	end
	
	function _item:OnWork()
		debug.Log(0, ' OnWork')
		
	end
	
	_item.btn_work.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnWork, _item))
	goUtil.SetParent(_item.gameObject, _ui.scr_builds_content)
	
	return _item
end

function _M:ctor()
	self.builds = {}
	self.items = {}
end

function _M:OnLoaded()
	events.AddListener(event.AssetChange, self.OnBuildChange, self)
	
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
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowItems()
	local items = cf_item.GetAllIndex()
	--local builds = m_build.GetBuilds()	
	local count = #items
	
	local hasNum = #self.items
	
	debug.LogFormat(0, 'count %d', count)
	debug.LogFormat(0, 'hasNum %d', hasNum)
	
	if count > hasNum then
		for i = hasNum, count, 1 do
			local item = NewItem(self)
			table.insert(self.items, item)
		end
	elseif count < hasNum then		
		for i = count, hasNum, 1 do
			local item = self.items[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.items[i]
		item:SetData(items[i])
	end
end

function _M:ShowBuilds()	
	local count = 3
	
	local hasNum = #self.builds
	
	debug.LogFormat(0, 'count %d', count)
	debug.LogFormat(0, 'hasNum %d', hasNum)
	
	if count > hasNum then
		for i = hasNum, count, 1 do
			local item = NewBuild(self)
			table.insert(self.builds, item)
		end
	elseif count < hasNum then		
		for i = count, hasNum, 1 do
			local item = self.builds[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local item = self.builds[i]
		item:SetActive(true)
		item:SetData(false)
	end
end

function _M:OnBuildChange()
	self:ShowBuilds()
end

return _M 