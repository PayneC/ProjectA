local cf_ui = require('configs/cf_ui')
local cf_formula = require('configs/cf_formula')
local cf_item = require('configs/cf_item')
local cf_weapon = require('configs/cf_weapon')

local c_workbench = require('controls/c_workbench')

local m_item = require('models/m_item')

local debug = require('base/debug')
local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')

local uibase = require('uis/ui_base')

local Vector3 = UnityEngine.Vector3

local _M = class(uibase)

local function NewItem(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_formula)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_name')	
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')	
	
	local _item = {		
	gameObject = gameObject,	
	spr_icon_ImageEx = spr_icon_ImageEx,
	txt_name_TextEx = txt_name_TextEx,
	txt_num_TextEx = txt_num_TextEx,
	
	DID = 0,	
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			self.spr_icon_ImageEx:SetSprite('item', cf_weapon.GetData(self.DID, cf_weapon.icon))
			self.txt_name_TextEx.text = cf_weapon.GetData(self.DID, cf_weapon.name)
			local count = m_item.GetItemCount(self.DID)
			self.txt_num_TextEx.text = string.format('X%d', count)
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	goUtil.SetParent(_item.gameObject, _ui.scr_formula_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	
	return _item
end


function _M:ctor()
	self.items = {}
end

function _M:OnLoaded()
	self.rt_formula = goUtil.FindChild(self.gameObject, 'rt_formula')
	self.scr_formula_content = goUtil.FindChild(self.gameObject, 'scr_formula/Viewport/Content')
	
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.btn_close.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
end

function _M:OnEnable()
	self:ShowItems()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowItems()
	local items = m_item.GetAllWeapon()
	local count = #items
	local hasNum = #self.items
	
	debug.LogFormat(0, 'ShowItems count %d', count)
	debug.LogFormat(0, 'ShowItems hasNum %d', hasNum)
	
	local item
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			item = NewItem(self)
			table.insert(self.items, item)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			item = self.items[i]
			item:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		item = self.items[i]
		item:SetData(items[i].DID)
	end
end

function _M:Close()
	uimgr.CloseUI(cf_ui.bag)
end

return _M 