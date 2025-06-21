import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';

const MatchResultInput = () => {
    const { id } = useParams();
    const [match, setMatch] = useState(null);
    const [teamStats, setTeamStats] = useState([]);
    const [playerStats, setPlayerStats] = useState([]);
    const [score, setScore] = useState({ team1Score: 0, team2Score: 0 });

    useEffect(() => {
        const fetchMatch = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}`);
                setMatch(response.data);
                if (response.data.team1Score !== undefined && response.data.team2Score !== undefined) {
                    setScore({
                        team1Score: response.data.team1Score,
                        team2Score: response.data.team2Score
                    });
                }
            } catch (error) {
                console.error("Ошибка загрузки матча:", error);
            }
        };

        const fetchTeamStats = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}/team-stats`);
                setTeamStats(response.data.$values || []);
            } catch (error) {
                console.error("Ошибка загрузки статистики команд:", error);
            }
        };

        const fetchPlayerStats = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/match/${id}/player-stats`);
                setPlayerStats(response.data.$values || []);
            } catch (error) {
                console.error("Ошибка загрузки статистики игроков:", error);
            }
        };

        fetchMatch();
        fetchTeamStats();
        fetchPlayerStats();
    }, [id]);

    const handleTeamStatChange = (teamId, statId, value) => {
        setTeamStats(prevStats =>
            prevStats.map(team => {
                if (team.teamId !== teamId) return team;
                return {
                    ...team,
                    stats: {
                        $values: team.stats.$values.map(stat =>
                            stat.statId === statId
                                ? { ...stat, value: parseInt(value) || 0 }
                                : stat
                        )
                    }
                };
            })
        );
    };

    const handlePlayerStatChange = (index, value) => {
        setPlayerStats(prev => {
            const updated = [...prev];
            updated[index].value = parseInt(value) || 0;
            return updated;
        });
    };

    const handleScoreChange = (team, value) => {
        setScore(prev => ({
            ...prev,
            [team]: parseInt(value) || 0
        }));
    };

    const saveStats = async () => {
        try {
            const token = sessionStorage.getItem("token");

            const flattenedTeamStats = teamStats.flatMap(team =>
                (team.stats.$values || []).map(stat => ({
                    teamId: team.teamId,
                    statId: stat.statId,
                    value: stat.value
                }))
            );

            const normalizedPlayerStats = playerStats.map(stat => ({
                playerId: stat.playerId,
                statId: stat.statId,
                value: stat.value
            }));

            await axios.post(`http://localhost:5082/api/match/${id}/update-stats`, {
                teamStats: flattenedTeamStats,
                playerStats: normalizedPlayerStats
            }, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });

            alert('Статистика успешно сохранена');
        } catch (error) {
            console.error("Ошибка при сохранении статистики:", error);
            alert("Ошибка при сохранении статистики");
        }
    };




    const saveScore = async () => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/match/${id}/score`, {
                team1Score: score.team1Score,
                team2Score: score.team2Score
            }, {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            });
            alert('Счёт успешно сохранён');
        } catch (error) {
            console.error("Ошибка при сохранении счёта:", error);
            alert("Ошибка при сохранении счёта");
        }
    };

    return (
        <div>
            <h1>Редактирование статистики матча #{id}</h1>

            <h2>Счёт матча</h2>
            {match && (
                <div>
                    <label>{match.team1?.name}:</label>
                    <input
                        type="number"
                        value={score.team1Score}
                        onChange={e => handleScoreChange("team1Score", e.target.value)}
                    />
                    <label>{match.team2?.name}:</label>
                    <input
                        type="number"
                        value={score.team2Score}
                        onChange={e => handleScoreChange("team2Score", e.target.value)}
                    />
                    <button onClick={saveScore}>Сохранить счёт</button>
                </div>
            )}

            <h2>Командная статистика</h2>
            {teamStats.map(team => (
                <div key={team.teamId}>
                    <h3>{team.teamName}</h3>
                    {team.stats.$values.map(stat => (
                        <div key={`${team.teamId}-${stat.statId}`}>
                            <label>{stat.statName}: </label>
                            <input
                                type="number"
                                value={stat.value}
                                onChange={e => handleTeamStatChange(team.teamId, stat.statId, e.target.value)}
                            />
                        </div>
                    ))}
                </div>
            ))}

            <h2>Игроки</h2>
            {playerStats.map((stat, idx) => (
                <div key={idx}>
                    <strong>{stat.playerName} - {stat.statName}:</strong>
                    <input
                        type="number"
                        value={stat.value}
                        onChange={e => handlePlayerStatChange(idx, e.target.value)}
                    />
                </div>
            ))}

            <button onClick={saveStats}>Сохранить статистику</button>
        </div>
    );
};

export default MatchResultInput;
