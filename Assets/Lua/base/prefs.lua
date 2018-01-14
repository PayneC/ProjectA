local PlayerPrefs = UnityEngine.PlayerPrefs

local _M = {}

local _isDirty = false

function _M.IsDirty()
	return _isDirty
end

function _M.SetTable(key, tab)
	_isDirty = true
	local sKey = tostring(key)
	local sValue = serialize(tab)
	PlayerPrefs.SetString(sKey, sValue)
end

function _M.GetTable(key)
	local sKey = tostring(key)
	if PlayerPrefs.HasKey(sKey) then
		local sValue = PlayerPrefs.GetString(sKey)
		local tab = unserialize(sValue)
		return tab
	end
end

function _M.Update()
	if _isDirty then
		PlayerPrefs.Save()
		_isDirty = false
	end
end

return _M 