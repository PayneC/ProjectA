local uibase = require('uis/ui_base')
local cf_ui = require('configs/cf_ui')

local cf_formula = require('csv/cf_formula')

local c_workbench = require('controls/c_workbench')
local c_build = require('controls/c_build')

local m_formula = require('models/m_formula')
local m_trade = require('models/m_trade')
local m_workbench = require('models/m_workbench')
local m_build = require('models/m_build')

local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local events = require('base/events')

local eventType = require('misc/event_type')
local common = require('misc/common')
local constant = require('misc/constant')

local Vector3 = UnityEngine.Vector3

local _M = class(uibase)

local function NewStuff(go, _ui)
	local _stuff = {
		gameObject = go,
		UIGridElement = false,
		spr_icon_ImageEx = false,
		txt_num_TextEx = false,
		btn_func_ButtonEx = false,
		txt_func_TextEx = false,
		spr_func_ImageEx = false,
		
		formulaID = 0,
		stuffID = 0,
		num = 0,
		func = 0, -- 0， 1-收取， 2-制作， 3-交易， 4-钻石
		value = 0,
		value2 = 0,
		ui = _ui,
	}
	
	_stuff.UIGridElement = goUtil.GetComponent(_stuff.gameObject, typeof(UIGridElement), nil)
	_stuff.spr_icon_ImageEx = goUtil.GetComponent(_stuff.gameObject, typeof(ImageEx), 'spr_icon')
	_stuff.txt_num_TextEx = goUtil.GetComponent(_stuff.gameObject, typeof(TextEx), 'txt_num')
	_stuff.btn_func_ButtonEx = goUtil.GetComponent(_stuff.gameObject, typeof(ButtonEx), 'btn_func')
	_stuff.txt_func_TextEx = goUtil.GetComponent(_stuff.gameObject, typeof(TextEx), 'txt_func')
	_stuff.spr_func_ImageEx = goUtil.GetComponent(_stuff.gameObject, typeof(ImageEx), 'spr_func')
	
	function _stuff:SetData(formulaID)
		self.formulaID = formulaID
		self:RefreshStuff()
	end
	
	function _stuff:RefreshStuff()
		local formula = m_formula.FindFormula(self.formulaID)
		if not formula then
			return
		end
		local stuffs = formula.overrideStuff or cf_formula.GetData(self.formulaID, cf_formula.stuff)
		local index = self.UIGridElement:GetIndex() + 1
		local stuff = stuffs and stuffs[index]
		
		self.DID = stuff and stuff[1]
		self.num = stuff and stuff[2]
		if self.DID then
			goUtil.SetActive(self.gameObject, true)
			common.SetItemIcon(self.spr_icon_ImageEx, self.DID)
			
			self:RefreshNum()
		else
			goUtil.SetActive(self.gameObject, false)
		end
	end
	
	function _stuff:RefreshNum()
		func = 0
		local has = common.GetItemCount(self.DID)
		if has >= self.num then
			self.txt_num_TextEx.text = string.format('%d/%d', has, self.num)
		else
			self.txt_num_TextEx.text = string.format('<color=red>%d</color>/%d', has, self.num)
			
			local t1, t2 = common.GetItemType(self.DID)
			if t2 == constant.Item_Stuff then
				local build = m_build.GetItemCorrelationBuild(self.DID)
				local count = build and build.count or 0
				if count > 0 then
					func = 1
					value = build.UID
				else
					local formula = m_formula.GetItemCorrelationFormula(self.DID)
					local stuffs
					if formula then
						stuffs = formula.overrideStuff or cf_formula.GetData(formula.DID, cf_formula.stuff)
					end
					if formula and common.CheckCosts(stuffs) then
						func = 2
						value = formula.DID
					else
						local coin, cash = common.GetItemPrice(self.DID)
						if cash then
							func = 4
							value2 = self.num - has
							value = cash * value2
						else
							func = 0
						end
					end
				end
			elseif t2 == constant.Item_Weapon then
				local formula = m_formula.GetItemCorrelationFormula(self.DID)
				local stuffs
				if formula then
					stuffs = formula.overrideStuff or cf_formula.GetData(formula.DID, cf_formula.stuff)
				end
				if formula and common.CheckCosts(stuffs) then
					func = 2
					value = formula.DID
				else
					func = 3
				end
			else
				func = 0
			end
			--Todo
		end
		
		if func == 0 then
			goUtil.SetActiveByComponent(self.btn_func_ButtonEx, false)
			goUtil.SetActiveByComponent(self.txt_func_TextEx, false)
		else
			goUtil.SetActiveByComponent(self.btn_func_ButtonEx, true)
			goUtil.SetActiveByComponent(self.txt_func_TextEx, true)
			if func == 1 then
				self.txt_func_TextEx.text = '收取'
			elseif func == 2 then
				self.txt_func_TextEx.text = '制作'
			elseif func == 3 then
				self.txt_func_TextEx.text = '交易'
			elseif func == 4 then
				self.txt_func_TextEx.text = string.format('  %s', value)
			end
		end
		
		if func == 4 then
			goUtil.SetActiveByComponent(self.spr_func_ImageEx, true)
		else	
			goUtil.SetActiveByComponent(self.spr_func_ImageEx, false)
		end	
	end
	
	function _stuff:GetIndex()
		return self.UIGridElement:GetIndex()
	end
	
	function _stuff:OnItemChange(DID)
		self:RefreshNum()
	end
	
	function _stuff:OnIndexChange()
		self:RefreshStuff()
	end
	
	function _stuff:OnBtnFunc()
		if func == 0 then
			
		elseif func == 1 then
			c_build.CollectStuff(value)
		elseif func == 2 then
			c_workbench.MakeFormula(self.ui.workbenchID, value)
			self.ui.workbenchID = m_workbench.GetEmptyBench()
			if not self.ui.workbenchID then
				uimgr.CloseUI(cf_ui.formula_lack)
				uimgr.CloseUI(cf_ui.formula)
			end
		elseif func == 3 then
			local sell = m_trade.GetItemCorrelationSell(self.DID)
			uimgr.OpenSubUI(cf_ui.trade_item, sell.UID)
		elseif func == 4 then
			common.CashBuy(self.DID, value2)
		end
	end
	
	_stuff.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_stuff.OnIndexChange, _stuff))
	_stuff.btn_func_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_stuff.OnBtnFunc, _stuff))
	
	return _stuff
end

function _M:ctor()
	self.btn_close = false
	self.btn_bg = false
	self.btn_make_ButtonEx = false
	self.scr_stuffs_UIGrid = false
	self.rt_stuff = false
	
	self.spr_icon_ImageEx = false
	self.txt_name_TextEx = false
	
	self.stuffs = {}
	self.workbenchID = false
	self.formulaID = false
end

function _M:OnLoaded()
	self.scr_stuffs_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_stuffs')	
	self.rt_stuff = goUtil.FindChild(self.gameObject, 'rt_stuff')
	self.btn_close = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
	
	self.spr_icon_ImageEx = goUtil.GetComponent(self.gameObject, typeof(ImageEx), 'spr_icon')
	self.txt_name_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_name')
	
	self.btn_make_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_make')
	self.btn_make_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnMake, self))
	
	self.btn_close.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
	self.scr_stuffs_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))
end

--@parameter:{[1] = workbenchID, [2] = formulaID}
function _M:OnEnable(parameter)
	self.workbenchID = parameter[1]
	self.formulaID = parameter[2]
	
	self:ShowStuffs()
	
	if self:CheckSufficient() then
		self.btn_make_ButtonEx.interactable = true
	else
		self.btn_make_ButtonEx.interactable = false
	end
	
	self.OnStuffChangeHandler = events.AddListener(eventType.StuffChange, self.OnItemChange, self)
	self.OnItemChangeHandler = events.AddListener(eventType.ItemChange, self.OnItemChange, self)
	self.OnFormulaChangeHandler = events.AddListener(eventType.FormulaChange, self.OnFormulaChange, self)
end

function _M:Update(dt)
	c_build.CalculateAll()
end

function _M:OnDisable()
	events.RemoveListener(eventType.StuffChange, self.OnStuffChangeHandler)
	events.RemoveListener(eventType.ItemChange, self.OnItemChangeHandler)
	events.RemoveListener(eventType.FormulaChange, self.OnFormulaChangeHandler)
end

function _M:OnDestroy()
	
end

function _M:OnAddNewElement(go)
	local stuff = NewStuff(go, self)
	table.insert(self.stuffs, stuff)
end

function _M:OnItemChange(DID)
	for i = 1, #self.stuffs, 1 do
		self.stuffs[i]:OnItemChange(DID)
	end
	
	if self:CheckSufficient() then
		self.btn_make_ButtonEx.interactable = true
	else
		self.btn_make_ButtonEx.interactable = false
	end
end

function _M:OnFormulaChange(DID)
	if self.formulaID == DID then
		self:ShowStuffs()
	end
end

function _M:Close()
	uimgr.CloseUI(cf_ui.formula_lack)
end


function _M:OnMake()
	if self:CheckSufficient() then
		c_workbench.MakeFormula(self.workbenchID, self.formulaID)
		self.workbenchID = m_workbench.GetEmptyBench()
		if not self.workbenchID then
			uimgr.CloseUI(cf_ui.formula_lack)
			uimgr.CloseUI(cf_ui.formula)
		end
	else
		goUtil.SetActiveByComponent(self.btn_make_ButtonEx, false)
	end
end

function _M:ShowStuffs()
	local formula = m_formula.FindFormula(self.formulaID)
	
	common.SetItemIcon(self.spr_icon_ImageEx, formula.itemID)
	common.SetItemName(self.txt_name_TextEx, formula.itemID)
	
	local stuffs = formula.overrideStuff or cf_formula.GetData(self.formulaID, cf_formula.stuff)
	local count = stuffs and #stuffs or 0
	self.scr_stuffs_UIGrid:SetElementCount(count)
	self.scr_stuffs_UIGrid:ApplySetting()
	
	local stuff
	for i = 1, #self.stuffs, 1 do
		stuff = self.stuffs[i]
		if stuff then
			stuff:SetData(self.formulaID)
		end
	end
end

function _M:CheckSufficient()
	local formula = m_formula.FindFormula(self.formulaID)
	local stuffs = formula and formula.overrideStuff or cf_formula.GetData(self.formulaID, cf_formula.stuff)
	return common.CheckCosts(stuffs)
end

return _M 