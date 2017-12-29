local debug = require('base/debug')
local common = require('commons/common')

local _M = {
	id = 1,
	careers = 2,
	name = 3,
	asset = 4
}

local _datas = {
	[10001] = {10001, {0}, 'npc01', 'npc_blade_girl'},
	[10002] = {10002, {0}, 'npc02', 'skeleton_grunt'},
	[10003] = {10003, {0}, 'npc03', 'skeleton_grunt'},
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