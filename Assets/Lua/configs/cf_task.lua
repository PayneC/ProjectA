local debug = require('base/debug')
local common = require('commons/common')

local _M = {
}

local _datas = {
}

local _datasReadOnly = common.ReadOnly(_datas)

function _M.GetData(_id, _type)
	local _data = _M.GetDataEntire(_id)
	if _data then
		return _data[_type]
	end
	return nil
end

function _M.GetDataEntire(_id)
	local _data = _datas[_id]
	if _data then
		return _data
	end
	debug.LogErrorFormat('cf_bulid not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datasReadOnly
end

return _M 