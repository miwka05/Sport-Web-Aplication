import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const TeamList = () => {
    const [teams, setTeams] = useState([]);
    const [bannedTeams, setBannedTeams] = useState([]);

    useEffect(() => {
        axios.get('http://localhost:5082/api/teams/list')
            .then(res => {
                const all = res.data.$values || [];
                setTeams(all.filter(t => !t.ban));
                setBannedTeams(all.filter(t => t.ban));
            })
            .catch(err => console.error("Ошибка загрузки команд:", err));
    }, []);

    return (
        <div>
            <h2>Список команд</h2>
            <ul>
                {teams.map(team => (
                    <li key={team.id}>
                        <Link to={`/team/${team.id}`}>{team.name}</Link> — {team.city}, {team.sportName}
                    </li>
                ))}
            </ul>

            {bannedTeams.length > 0 && (
                <>
                    <h3>Заблокированные команды</h3>
                    <ul style={{ color: 'gray' }}>
                        {bannedTeams.map(team => (
                            <li key={team.id}>
                                <Link to={`/team/${team.id}`}>{team.name}</Link> — {team.city}, {team.sportName}
                                <br />
                                <strong>Причина блокировки:</strong> {team.reasonBan || 'не указана'}
                            </li>
                        ))}
                    </ul>
                </>
            )}
        </div>
    );
};

export default TeamList;

