local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	name = 2,
	icon = 3,
}

local _datas = {
	[10001] = {10001, '木', 'icon_mutou'},
	[10002] = {10002, '铁', 'icon_tie'},
	[10003] = {10003, '皮', 'icon_pi'},
	[10004] = {10004, '药', 'icon_yao'},
}

local _index = {
	10001, 10002, 10003, 10004
}

local _datasReadOnly = common.ReadOnly(_datas)
local _indexReadOnly = common.ReadOnly(_index)

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

function _M.GetAllIndex()
	return _index
end

return _M 