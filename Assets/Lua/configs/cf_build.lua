local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	itemID = 2,
	name = 3,
	icon = 4,
	cost = 5,
	countLimit = 6,
}

local _datas = {
	[10001] = {10001, 10001, '木桶', 'icon_build_mutong', {100, 200, 300}, 3},
	[10002] = {10002, 10002, '铁桶', 'icon_zuanshi', {100, 200, 300}, 3},
	[10003] = {10003, 10003, '皮桶', 'icon_build_mutong', {100, 200, 300}, 3},
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