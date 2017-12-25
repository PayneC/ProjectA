local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
}

local _datas = {
	[10001] = {10001, '木', 'icon_mutou'},
	[10002] = {10002, '铁', 'icon_mutou'},
	[10003] = {10003, '皮', 'icon_zuanshi'},
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
	return _datas
end

return _M 