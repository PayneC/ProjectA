local uibase = require('uis/ui_base')
local cf_ui = require('configs/cf_ui')

local cf_country = require('csv/cf_country')

--local m_country = require('models/m_country')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local time_mgr = require('base/time_mgr')

local eventType = require('misc/event_type')
local common = require('misc/common')
local constant = require('misc/constant')

local _M = class(uibase)

local function NewCountry(go, _ui)
    local _country = {
        gameObject = go,
        UIGridElement = false,
        btn_bg_ButtonEx = false,
        rt_items = false,
        txt_name_TextEx = false,
        txt_num_TextEx = false,
        txt_time_TextEx = false,

        DID = 0,
    }

    _country.UIGridElement = goUtil.GetComponent(_country.gameObject, typeof(UIGridElement), nil)
    _country.btn_bg_ButtonEx = goUtil.GetComponent(_country.gameObject, typeof(ButtonEx), 'btn_bg')
    _country.rt_items = goUtil.FindChild(_country.gameObject, 'rt_items')
    _country.txt_name_TextEx = goUtil.GetComponent(_country.gameObject, typeof(TextEx), 'txt_name')
    _country.txt_num_TextEx = goUtil.GetComponent(_country.gameObject, typeof(TextEx), 'txt_num')
    _country.txt_time_TextEx = goUtil.GetComponent(_country.gameObject, typeof(TextEx), 'txt_time')

    function _country:OnIndexChange()
        local index = _country.UIGridElement:GetIndex() + 1
        local DIDs = cf_country.GetAllIndex()
        local DID = DIDs[index]
        self:SetData(DID)
    end

    function _country:OnClick()
        _ui:OnCountryClick(self.DID)
    end

    function _country:SetData(DID)
        self.DID = DID

    end

    _country.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_country.OnIndexChange, _country))
    _country.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_country.OnClick, _country))

    return _country
end

function _M:ctor()
    self.btn_bg_ButtonEx = false
    self.btn_close_ButtonEx = false

    self.rt_item = false
    self.countrys = {}
    self.callback = false
end

function _M:OnLoaded()
    self.btn_bg_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bg')
    self.btn_close_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
    self.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
    self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))

    self.scr_builds_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_builds')
    self.scr_builds_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))

    self.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
    self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.Close, self))
end

function _M:OnEnable(callback)
    self:Refresh()
    self.callback = callback
end

function _M:Update(dt)

end

function _M:OnDisable()
    self.callback = false
end

function _M:OnDestroy()

end

function _M:OnAddNewElement(go)
    local country = NewCountry(go, self)
    table.insert(self.countrys, country)
end

function _M:Close()
    uimgr.CloseUI(cf_ui.caravan_info)
end

function _M:Refresh()
    local indexs = cf_country.GetAllIndex()
    self.scr_builds_UIGrid:SetElementCount(#indexs)
    self.scr_builds_UIGrid:ApplySetting()
end

function _M:OnCountryClick(DID)
    if self.callback then
        self.callback[1](self.callback[2], DID)
        uimgr.CloseUI(cf_ui.country)
    end
end

return _M