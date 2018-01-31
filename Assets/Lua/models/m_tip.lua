local _M = {}

local _entities = {}

function _M.PushEntity(itemID, count)
	table.insert(_entities, {itemID, count})
end

function _M.PopEntity()
	if #_entities > 0 then
		local entity = _entities[1]
		table.remove(_entities, 1)
		return entity
	end
end

return _M 