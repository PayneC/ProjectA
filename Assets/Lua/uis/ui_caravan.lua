local uibase = require('uis/ui_base')

local uimgr = require('base/ui_mgr')
local goUtil = require('base/goutil')
local time_mgr = require('base/time_mgr')

local common = require('misc/common')

local m_caravan = require('models/m_caravan')

local cf_ui = require('configs/cf_ui')

local cf_country = require('csv/cf_country')

local _M = class(uibase)

local function NewCaravan(go)
    local _caravan = {
        gameObject = go,
        UIGridElement = false,
        btn_bg_ButtonEx = false,
        spr_icon_ImageEx = false,
        txt_time_TextEx = false,

        UID = 0,
        countryID = 0,
        needTime = 0,
        startTime = 0,
    }

    _caravan.UIGridElement = goUtil.GetComponent(_caravan.gameObject, typeof(UIGridElement), nil)
    _caravan.btn_bg_ButtonEx = goUtil.GetComponent(_caravan.gameObject, typeof(ButtonEx), 'btn_bg')
    _caravan.spr_icon_ImageEx = goUtil.GetComponent(_caravan.gameObject, typeof(ImageEx), 'spr_icon')
    _caravan.txt_time_TextEx = goUtil.GetComponent(_caravan.gameObject, typeof(TextEx), 'txt_time')

    function _caravan:OnIndexChange()
        local index = self.UIGridElement:GetIndex() + 1
        local caravan = m_caravan.GetCaravanByIndex(index)
        self.UID = caravan.UID
        debug.LogFormat(0, 'OnIndexChange() UID = %d', caravan.UID)
        self:SetData(caravan)
    end

    function _caravan:OnCaravanChange(UID)
        if UID == self.UID then
            local caravan = m_caravan.FindCaravan(UID)
            self:SetData(caravan)
        end
    end

    function _caravan:OnClick()
        uimgr.OpenSubUI(cf_ui.caravan_info, self.UID)
    end

    function _caravan:SetData(caravan)
        debug.LogFormat(0, 'SetData(%d)', self.countryID)
        self.countryID = caravan.countryID
        if self.countryID and self.countryID ~= 0 then
            goUtil.SetActiveByComponent(self.spr_icon_ImageEx, true)
            common.SetCountryIcon(self.spr_icon_ImageEx, self.countryID)

            self.startTime = caravan.time or 0
            self.needTime = cf_country.GetData(self.countryID, cf_country.time) or 0
            local ct = time_mgr.GetTime() or 0
            self.txt_time_TextEx.text = time_mgr.TimeToString(self.needTime - ct + self.startTime)
        else
            goUtil.SetActiveByComponent(self.spr_icon_ImageEx, false)
            self.txt_time_TextEx.text = '空闲'
        end
    end

    function _caravan:Update(dt)
        if self.countryID and self.countryID ~= 0 then
            local ct = time_mgr.GetTime() or 0
            self.txt_time_TextEx.text = time_mgr.TimeToString(self.needTime - ct + self.startTime)
        end
    end

    _caravan.UIGridElement:AddIndexChangeListener(UnityEngine.Events.UnityAction(_caravan.OnIndexChange, _caravan))
    _caravan.btn_bg_ButtonEx.onClick:AddListener(UnityEngine.Events.UnityAction(_caravan.OnClick, _caravan))

    return _caravan
end

function _M:ctor()
    self.scr_builds_UIGrid = false

    self.caravans = {}
end

function _M:OnLoaded()
    self.scr_builds_UIGrid = goUtil.GetComponent(self.gameObject, typeof(UIGrid), 'scr_builds')
    self.scr_builds_UIGrid:AddNewElementListener(UnityEngine.Events.UnityAction_GameObject(self.OnAddNewElement, self))
end

function _M:OnEnable()
    self:Refresh()
end

function _M:Update(dt)

end

function _M:OnDisable()

end

function _M:OnDestroy()

end

function _M:OnAddNewElement(go)
    local caravan = NewCaravan(go)
    table.insert(self.caravans, caravan)
end

function _M:Refresh()
    local count = m_caravan.GetCaravanCount()
    self.scr_builds_UIGrid:SetElementCount(count)
    self.scr_builds_UIGrid:ApplySetting()
end

return _M