local events = require('base/events')
local eventType = require('misc/event_type')
local prefs = require('base/prefs')

local _isModify = false
local _caravans = {}

local _M = {}

local function NewCaravan(UID)
    local _caravan = {
        UID = UID or 0,
        countryID = 0,
        time = 0,
        items = false,
        itmeLimit = 5,
    }
    return _caravan
end

function _M.SetCaravanDirty(UID)
    _isModify = true

    events.Brocast(eventType.CaravanChange, UID)
end

function _M.AddCaravan(UID)
    debug.LogFormat(0, 'AddCaravan(%d)', UID)
    local caravan = NewCaravan(UID)
    if caravan then

        table.insert(_caravans, caravan)
        _isModify = true

        events.Brocast(eventType.CaravanAdd)
    end
    return caravan
end

function _M.FindCaravan(UID)
    local formula
    for i = 1, #_caravans, 1 do
        formula = _caravans[i]
        if formula.DID == DID then
            return formula
        end
    end
end

function _M.GetCaravanByIndex(index)
    if _caravans then
        return _caravans[index]
    else
        return nil
    end
end

function _M.GetCaravanCount()
    if _caravans then
        return #_caravans
    else
        return 0
    end
end

function _M.ReadData()
    _caravans = prefs.GetTable('_caravans') or {}
end

function _M.WriteData()
    if _isModify then
        prefs.SetTable('_caravans', _caravans)
        _isModify = false
    end
end

function _M.ParseData()

end

return _M