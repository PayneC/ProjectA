local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	itemID = 2,
	stuff = 3,
	timeCost = 4,
}

local _datas = {
	[10001] = {10001, 11001, {{10002, 3}}, 10},
	[10002] = {10002, 11002, {{10001, 2}, {10002, 1}}, 20},
	[10003] = {10003, 11003, {{10001, 1}, {10002, 2}}, 30},	
}

local _index = {
	10001, 10002, 10003
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