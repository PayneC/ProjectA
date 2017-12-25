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

local function NewBuild(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	--local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_name')
	local txt_lv_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_lv')
	
	local spr_item_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_item_icon')
	--local txt_item_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_name')
	local txt_item_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_num')
	
	local btn_self = goUtil.GetComponent(gameObject, typeof(ButtonEx), nil)
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		btn_self = btn_self,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_name_TextEx = txt_name_TextEx,
		txt_lv_TextEx = txt_lv_TextEx,
		spr_item_icon_ImageEx = spr_item_icon_ImageEx,
		txt_item_name_TextEx = txt_item_name_TextEx,
		txt_item_num_TextEx = txt_item_num_TextEx,
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(gameObject, true)
			local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
			--self.txt_name_TextEx.text = cf_build.GetData(self.DID, cf_build.name)	
			self.spr_icon_ImageEx:SetSprite('uimain', cf_build.GetData(self.DID, cf_build.icon))
			--self.txt_item_name_TextEx.text = cf_item.GetData(itemID, cf_item.name)	
			self.spr_item_icon_ImageEx:SetSprite('uimain', cf_item.GetData(itemID, cf_item.icon))
		else
			goUtil.SetActive(gameObject, false)
		end
	end
	
	goUtil.SetParent(gameObject, _ui.scr_items_content)
	
	return _item
end

function _M:ctor()
	self.txt_info = false
	self.builds = {}
	
	self.OnBuildChange = nil
end

function _M:OnLoaded()
	self.txt_info = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_info')
	events.AddListener(event.BuildChange, self.OnBuildChange, self)
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'scr_items/Viewport/Content')
end

function _M:OnEnable()
	self:ShowBuilds()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowBuilds()
	local builds = m_build.GetBuilds()	
	local count = #builds
	
	local hasNum = #self.builds
	
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
		item:SetData(builds[i])
	end
end

function _M:OnBuildChange()
	self:ShowBuilds()
end

return _M 