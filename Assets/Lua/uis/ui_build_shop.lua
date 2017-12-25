local cf_ui = require('configs/cf_ui')
local cf_build = require('configs/cf_build')
local cf_item = require('configs/cf_item')

local m_build = require('models/m_build')

local debug = require('base/debug')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')

local uibase = require('uis/ui_base')

local _M = class(uibase)

local function NewItem(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_item)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_name')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')
	
	local spr_item_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_item_icon')
	local txt_item_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_name')
	local txt_item_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_item_num')
	
	local btn_self = goUtil.GetComponent(gameObject, typeof(ButtonEx), nil)
	
	local _item = {
		DID = 0,
		gameObject = gameObject,
		btn_self = btn_self,
		spr_icon_ImageEx = spr_icon_ImageEx,
		txt_name_TextEx = txt_name_TextEx,
		txt_num_TextEx = txt_num_TextEx,
		spr_item_icon_ImageEx = spr_item_icon_ImageEx,
		txt_item_name_TextEx = txt_item_name_TextEx,
		txt_item_num_TextEx = txt_item_num_TextEx,
	}
	
    local function OnClick()
        debug.Log(0, ' OnClick')
		m_build.AddBuild(_item.DID)
	end
	
	_item.btn_self.onClick:AddListener(OnClick)
	
	function _item:SetData(DID)
		self.DID = DID	
		local itemID = cf_build.GetData(self.DID, cf_build.itemID)	
		self.txt_name_TextEx.text = cf_build.GetData(self.DID, cf_build.name)	
		self.spr_icon_ImageEx:SetSprite('uimain', cf_build.GetData(self.DID, cf_build.icon))
		self.txt_item_name_TextEx.text = cf_item.GetData(itemID, cf_item.name)	
		self.spr_item_icon_ImageEx:SetSprite('uimain', cf_item.GetData(itemID, cf_item.icon))
	end
	
	return _item
end

local function LoadData(_ui)
	debug.Log(0, 'load data')
	local _datas = cf_build.GetAllDatas()
	debug.Log(0, debug.TableToString(_datas))
	for k, v in pairs(_datas) do
		local _item = NewItem(_ui)
		_item:SetData(k)
		goUtil.SetActive(_item.gameObject, true)
		goUtil.SetParent(_item.gameObject, _ui.scr_items_content)
	end
end

function _M:ctor()
	self.btn_close = nil
	self.rt_item = nil
	self.scr_items_content = nil
	
	self.OnClose = nil
end

function _M:OnLoaded()
	local function OnClose()
		uimgr.CloseUI(cf_ui.buildShop)
	end
	
	self.OnClose = OnClose
	
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	self.btn_close.onClick:AddListener(OnClose)
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'scr_items/Viewport/Content')
	
	LoadData(self)
end

function _M:OnEnable()
	
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	if self.OnClose then
		self.btn_close.onClick:RemoveListener(self.OnClose)
	end
end

return _M 