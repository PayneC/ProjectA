local uibase = require('uis/ui_base')

local cf_ui = require('configs/cf_ui')

local time_mgr = require('base/time_mgr')
local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')

local common = require('misc/common')
local constant = require('misc/constant')

local m_caravan = require('models/m_caravan')

local c_caravan = require('controls/c_caravan')

local cf_country = require('csv/cf_country')

local _M = class(uibase)

local function NewItem(go, _ui)
    local _item = {
        gameObject = go,
        UIGridElement = false,
        btn_bg_ButtonEx = false,
        spr_icon_ImageEx = false,

        DID = 0,
        index = 0,
    }

    _item.UIGridElement = goUtil.GetComponent(_item.gameObject, typeof(UIGridElement), nil)
    _item.btn_bg_ButtonEx = goUtil.GetComponent(_item.gameObject, typeof(ButtonEx), 'btn_bg')
    _item.spr_icon_ImageEx = goUtil.GetComponent(_item.gameObject, typeof(ImageEx), 'spr_icon')

    function _item:OnIndexChange()
        self.index = self.UIGridElement:GetIndex() + 1
        local DID = _ui:GetItem(self.index)
        self:SetData(DID)
    end

    function _item:OnSelectItemChange(DID)
        _ui:SetItem(self.index, self.DID)
        self:SetData(DID)
    end

    function _item:OnClick()
        if self.DID and self.DID ~= 0 then
            self:SetData(0)
        else
            uimgr.OpenSubUI(cf_ui.bag, { self.OnSelectItemChange, self })
        end
    end

    function _item:SetData(DID)
        self.DID = DID
        common.SetItemIcon(self.spr_icon_ImageEx, self.DID)
    end

    _item.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_item.OnIndexChange, _item))
    _item.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_item.OnClick, _item))
end

function _M:ctor()
    self.btn_bg_ButtonEx = false
    self.btn_close_ButtonEx = false
    self.btn_caravan_ButtonEx = false
    self.btn_go_ButtonEx = false
    self.scr_items_UIGrid = false
    self.rt_caravan = false
    self.spr_icon_ImageEx = false
    self.txt_name_TextEx = false
    self.txt_coin_TextEx = false
    self.txt_time_TextEx = false

    self.UID = 0
    self.countryID = 0
    self.items = {}
    self.itemCount = 0
    self.needCoin = 0
end

function _M:OnLoaded()
    self.btn_bg_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_bg')
    self.btn_close_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_close')
    self.btn_caravan_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_caravan')
    self.btn_go_ButtonEx = goUtil.GetComponent(self.gameObject, typeof(ButtonEx), 'btn_go')
    self.scr_items_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_items')
    self.rt_caravan = goUtil.FindChild(self.gameObject, 'rt_caravan')
    self.spr_icon_ImageEx = goUtil.GetComponent(self.gameObject, typeof(ImageEx), 'rt_caravan/spr_icon')
    self.txt_name_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'rt_caravan/txt_name')
    self.txt_coin_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'rt_caravan/txt_coin')
    self.txt_time_TextEx = goUtil.GetComponent(self.gameObject, typeof(TextEx), 'txt_time')

    self.scr_items_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))
    self.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
    self.btn_close_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnClose, self))
    self.btn_caravan_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(self.OnCaravan, self))
end

function _M:OnEnable(UID)
    debug.LogFormat(0, 'OnEnable(%d)', UID)
    self.UID = UID
    self:Refresh()
end

function _M:Update(dt)

end

function _M:OnDisable()
    for i = 1, self.itemCount, 1 do
        self.items[i] = 0
    end
end

function _M:OnDestroy()

end

function _M:OnClose()
    uimgr.CloseUI(cf_ui.caravan_info)
end

function _M:OnAddNewElement(go)
    local item = NewItem(go, self)
    table.insert(self.items, item)
end

function _M:OnSelectCountry(DID)
    self.countryID = DID
    self:RefreshCountry()
end

function _M:OnCaravan()
    uimgr.OpenSubUI(cf_ui.country, { self.OnSelectCountry, self })
end

function _M:SetItem(index, DID)
    self.items[index] = DID or 0
end

function _M:GetItem(index)
    return self.items[index] or 0
end

function _M:Refresh()
    local caravan = m_caravan.FindCaravan(self.UID)
    self.itemCount = caravan.itmeLimit or 0
    for i = 1, self.itemCount, 1 do
        self.items[i] = 0
    end

    self.scr_items_UIGrid:SetElementCount(self.itemCount)
    self.scr_items_UIGrid:ApplySetting()
end

function _M:RefreshCountry()
    debug.LogFormat(0, 'RefreshCountry() self.countryID = %d', self.countryID)
    if self.countryID and self.countryID ~= 0 then
        goUtil.SetActive(self.rt_caravan, true)
        common.SetCountryIcon(self.spr_icon_ImageEx, self.countryID)
        common.SetCountryName(self.txt_name_TextEx, self.countryID)
        self.needCoin = cf_country.GetData(self.countryID, cf_country.coin)
    else
        goUtil.SetActive(self.rt_caravan, false)
    end
end

return _M