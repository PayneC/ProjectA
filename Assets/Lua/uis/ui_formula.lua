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
local events = require('base/events')
local define = require('commons/define')
local eventType = require('commons/event_type')

local uibase = require('uis/ui_base')

local Vector3 = UnityEngine.Vector3

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
			self.spr_icon_ImageEx:SetSprite('item', cf_item.GetData(self.DID, cf_item.icon))
		else
			goUtil.SetActive(self.gameObject, false)
		end
		self:RefreshData()
	end
	
	function _item:RefreshData()		
		local count = m_item.GetItemCount(self.DID)
		local storage = m_item.GetItemStorage(self.DID)
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

local function NewStuff(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_stuff)
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_num_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_num')	
	
	local _item = {		
	gameObject = gameObject,
	spr_icon_ImageEx = spr_icon_ImageEx,
	txt_num_TextEx = txt_num_TextEx,
	
	DID = 0,
	}
	
	function _item:SetData(DID, num)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			self.spr_icon_ImageEx:SetSprite('item', cf_item.GetData(self.DID, cf_item.icon))
			self.txt_num_TextEx.text = string.format('%d', num)			
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	return _item
end

local function NewFormula(_ui)
	local gameObject = goUtil.Instantiate(_ui.rt_formula)
	local rt_stuff = goUtil.FindChild(gameObject, 'rt_stuff')
	local spr_icon_ImageEx = goUtil.GetComponent(gameObject, typeof(ImageEx), 'spr_icon')
	local txt_name_TextEx = goUtil.GetComponent(gameObject, typeof(TextEx), 'txt_name')	
	local btn_make = goUtil.GetComponent(gameObject, typeof(ButtonEx), 'btn_make')
	
	local _item = {		
	gameObject = gameObject,
	rt_stuff = rt_stuff,
	spr_icon_ImageEx = spr_icon_ImageEx,
	txt_name_TextEx = txt_name_TextEx,
	btn_make = btn_make,
	
	DID = 0,
	stuffs = {},
	}
	
	function _item:SetData(DID)
		self.DID = DID	
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			local itemID = cf_formula.GetData(self.DID, cf_formula.itemID)	
			self.spr_icon_ImageEx:SetSprite('item', cf_weapon.GetData(itemID, cf_weapon.icon))
			
			self.txt_name_TextEx.text = cf_weapon.GetData(itemID, cf_weapon.name)
			
			self:ShowStuffs()
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _item:ShowStuffs()
		local stuffs = cf_formula.GetData(self.DID, cf_formula.stuff)
		debug.LogFormat(0, 'ShowStuffs stuffs count = %d', #stuffs)
		if stuffs then
			for i = 1, #stuffs, 1 do
				local stuff = NewStuff(_ui)
				stuff:SetData(stuffs[i] [1], stuffs[i] [2])
				table.insert(self.stuffs, stuff)
				goUtil.SetParent(stuff.gameObject, self.rt_stuff)
				goUtil.SetLocalScale(stuff.gameObject, Vector3.one)
			end
		end
	end
	
	function _item:OnMake()
		c_workbench.MakeFormula(_ui.workbenchID, self.DID)
		uimgr.CloseUI(cf_ui.formula)
	end
	
	goUtil.SetParent(_item.gameObject, _ui.scr_formula_content)
	goUtil.SetLocalScale(_item.gameObject, Vector3.one)
	btn_make.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnMake, _item))
	
	return _item
end


function _M:ctor()
	self.formulas = {}
	self.items = {}
	
	self.workbenchID = 0
end

function _M:OnLoaded()
	self.rt_formula = goUtil.FindChild(self.gameObject, 'rt_formula')
	self.scr_formula_content = goUtil.FindChild(self.gameObject, 'scr_formula/Viewport/Content')
	
	self.rt_stuff = goUtil.FindChild(self.gameObject, 'rt_stuff')
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.rt_item = goUtil.FindChild(self.gameObject, 'rt_item')
	self.scr_items_content = goUtil.FindChild(self.gameObject, 'rt_items')
	
	self.btn_close.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
	
	self:ShowFormulas()
	
	events.AddListener(eventType.ItemChange, self.OnItemChange, self)
end

function _M:OnEnable(workbenchID)
	self.workbenchID = workbenchID
	self:ShowItems()
end

function _M:Update(dt)
	
end

function _M:OnDisable()
	
end

function _M:OnDestroy()
	
end

function _M:ShowFormulas()
	local formulas = cf_formula.GetAllIndex()
	local count = #formulas
	local hasNum = #self.formulas
	
	debug.LogFormat(0, 'ShowFormulas count %d', count)
	debug.LogFormat(0, 'ShowFormulas hasNum %d', hasNum)
	
	if count > hasNum then
		for i = hasNum + 1, count, 1 do
			local formula = NewFormula(self)
			table.insert(self.formulas, formula)
		end
	elseif count < hasNum then		
		for i = count + 1, hasNum, 1 do
			local formula = self.formulas[i]
			formula:SetData(false)
		end
	end
	
	for i = 1, count, 1 do
		local formula = self.formulas[i]
		formula:SetData(formulas[i])
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

function _M:Close()
	uimgr.CloseUI(cf_ui.formula)
end

function _M:OnItemChange()
	self:ShowItems()
end

return _M 