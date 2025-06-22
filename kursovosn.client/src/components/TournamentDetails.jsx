import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import { getTournamentById, getParticipants } from '../services/tournamentService';
import axios from 'axios';

const TournamentDetails = () => {
    const { id } = useParams();
    const [tournament, setTournament] = useState(null);
    const [participants, setParticipants] = useState([]);
    const [canRegister, setCanRegister] = useState(false);
    const [matches, setMatches] = useState([]);
    const [teams, setTeams] = useState([]);
    const [selectedTeamId, setSelectedTeamId] = useState('');
    const [teamHasApplied, setTeamHasApplied] = useState(false);
    const [teamIsParticipant, setTeamIsParticipant] = useState(false);
    const [standings, setStandings] = useState([]);
    const [bracket, setBracket] = useState([]);
    const [info, setInfo] = useState('');
    const [userSchedule, setUserSchedule] = useState([]);
    const [banReason, setBanReason] = useState('');
    const [isAdmin, setIsAdmin] = useState(false);
    const [currentUserId, setCurrentUserId] = useState(null);
    const [registerError, setRegisterError] = useState('');

    useEffect(() => {
        getTournamentById(id)
            .then(setTournament)
            .catch((error) => {
                console.error(error);
                setTournament(null);
            });

        getParticipants(id)
            .then((data) => {
                if (data?.$values) {
                    setParticipants(data.$values);
                } else {
                    setParticipants(data);
                }
            })
            .catch((error) => {
                console.error("Ошибка при загрузке участников:", error);
            });

        const checkEligibility = async () => {
            try {
                const token = sessionStorage.getItem("token");
                const response = await axios.get(`http://localhost:5082/api/tournament/${id}/can-register`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (response.data === "ok") {
                    setCanRegister(true);
                } else {
                    setRegisterError("Вы не можете зарегистрироваться.");
                }
            } catch (error) {
                console.error("Ошибка проверки возможности регистрации", error);
                if (error.response && error.response.data) {
                    setRegisterError(error.response.data);
                } else {
                    setRegisterError("Ошибка проверки регистрации");
                }
                setCanRegister(false);
            }
        };

        const fetchMatches = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/tournament/${id}/matches`);
                const sorted = response.data.$values.sort((a, b) => new Date(a.date) - new Date(b.date));
                setMatches(sorted);
            } catch (error) {
                console.error("Ошибка загрузки матчей:", error);
            }
        };

        const fetchUserSchedule = async () => {
            try {
                const token = sessionStorage.getItem("token");
                const response = await axios.get(`http://localhost:5082/api/tournament/${id}/userMatches`, {
                    headers: { Authorization: `Bearer ${token}` }
                });

                const userId = JSON.parse(atob(token.split('.')[1])).nameid;

                // фильтрация и сортировка
                const schedule = response.data.$values
                    .filter(match =>
                        match.player1Id === userId || match.player2Id === userId ||
                        teams.some(t => t.id === match.team1Id || t.id === match.team2Id)
                    )
                    .sort((a, b) => new Date(a.date) - new Date(b.date)); // ⬅ сортировка по дате

                setUserSchedule(schedule);
            } catch (error) {
                console.error("Ошибка загрузки личного расписания:", error);
            }
        };

        const fetchStandings = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/tournament/${id}/standings`);
                setStandings(response.data.$values);
            } catch (error) {
                console.error("Ошибка загрузки турнирной таблицы:", error);
            }
        };


        const fetchBracket = async () => {
            try {
                const response = await axios.get(`http://localhost:5082/api/tournament/${id}/bracket`);
                setBracket(response.data.$values);
            } catch (error) {
                console.error("Ошибка загрузки сетки:", error);
            }
        };


        const fetchUserTeams = async () => {
            try {
                const token = sessionStorage.getItem("token");
                const response = await axios.get(`http://localhost:5082/api/teams/my`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setTeams(response.data.$values);
            } catch (error) {
                console.error("Ошибка загрузки команд пользователя:", error);
            }
        };

        const token = sessionStorage.getItem("token");
        if (token) {
            const payload = JSON.parse(atob(token.split('.')[1]));
            setCurrentUserId(payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
            if (payload && payload["sub"] === "admin") {
                setIsAdmin(true);
            }
        }

        checkEligibility();
        fetchMatches();
        fetchUserTeams();
        fetchStandings();
        fetchBracket();
        fetchUserSchedule();
    }, [id]);

    useEffect(() => {
        const checkTeamRequestAndParticipation = async () => {
            if (!selectedTeamId) return;
            try {
                const token = sessionStorage.getItem("token");
                const requestResponse = await axios.get(`http://localhost:5082/api/tournament/${id}/team-has-request/${selectedTeamId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setTeamHasApplied(requestResponse.data);

                const participationResponse = await axios.get(`http://localhost:5082/api/tournament/${id}/team-is-participant/${selectedTeamId}`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                setTeamIsParticipant(participationResponse.data);
            } catch (error) {
                console.error("Ошибка проверки команды:", error);
            }
        };

        checkTeamRequestAndParticipation();
    }, [selectedTeamId, id]);

    const handleRegister = async () => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post('http://localhost:5082/api/tournament/register', {
                tournamentId: id,
                info: info
            }, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert('Заявка успешно подана!');
        } catch (error) {
            console.error(error);
            alert('Ошибка при подаче заявки');
        }
    };

    const handleBanTournament = async () => {
        if (!banReason.trim()) {
            alert("Введите причину бана");
            return;
        }
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/tournament/${id}/ban`,
                { reasonBan: banReason },
                { headers: { Authorization: `Bearer ${token}` } }
            );
            alert("Турнир заблокирован");
        } catch (error) {
            console.error("Ошибка при блокировке турнира:", error);
            alert("Ошибка при блокировке турнира");
        }
    };

    const createBracket = async () => {
        try {
            const token = sessionStorage.getItem("token");
            await axios.post(`http://localhost:5082/api/tournament/${id}/generate-playoff`, {}, {
                headers: { Authorization: `Bearer ${token}` },
            });
            alert('Турнирная сетка успешно создана!');
            // После создания сетки, можно обновить таблицу матчей и турнирную таблицу
            //fetchMatches();
            //fetchStandings();
        } catch (error) {
            console.error("Ошибка при создании турнирной сетки:", error);
            alert('Ошибка при создании турнирной сетки');
        }
    };

    const handleTeamRegister = async () => {
        try {
            const team = teams.find(t => t.id === parseInt(selectedTeamId));
            if (!team || team.sport_ID !== tournament.sport_ID) {
                alert('Команда не соответствует виду спорта турнира.');
                return;
            }

            if (teamHasApplied) {
                alert('У выбранной команды уже есть активная заявка на этот турнир.');
                return;
            }

            if (teamIsParticipant) {
                alert('Эта команда уже участвует в турнире.');
                return;
            }

            const token = sessionStorage.getItem("token");
            await axios.post('http://localhost:5082/api/tournament/register', {
                tournamentId: id,
                info: info,
                teamId: parseInt(selectedTeamId)
            }, {
                headers: { Authorization: `Bearer ${token}` }
            });
            alert('Заявка от команды успешно подана!');
        } catch (error) {
            console.error(error);
            alert('Ошибка при подаче заявки от команды');
        }
    };

    if (!tournament) return <div>Загрузка...</div>;

    if (tournament?.ban === true && !isAdmin && tournament?.creator_ID !== currentUserId) {
        return (
            <div style={{ padding: "2rem", color: "red" }}>
                <h2>Этот турнир был заблокирован администрацией</h2>
                <p>Причина: {tournament.reason || 'не указана'}</p>
            </div>
        );
    }

    return (
        <div>
            <h1>{tournament.name}</h1>
            <p><strong>Вид спорта:</strong> {tournament.sport?.name}</p>
            <p><strong>Формат:</strong> {tournament.formatName}</p>
            <p><strong>Дата проведения:</strong> {new Date(tournament.start).toLocaleDateString()} – {new Date(tournament.end).toLocaleDateString()}</p>
            <p><strong>Адрес:</strong> {tournament.adress}</p>
            <p><strong>Возраст:</strong> {tournament.age}</p>
            <p><strong>Пол:</strong> {tournament.pol}</p>
            <p><strong>Информация:</strong> {tournament.info}</p>
            <p><strong>Статус:</strong> {tournament.status}</p>
            <p><strong>Одиночный турнир:</strong> {tournament.solo ? 'Да' : 'Нет'}</p>
            {tournament.creatorUserName && (
                <p>
                    <strong>Создатель турнира:</strong>{" "}
                    <Link to={`/profile/${tournament.creator_ID}`}>
                        {tournament.creatorUserName}
                    </Link>
                </p>
            )}
            {tournament?.creator_ID === currentUserId && (
                <Link to={`/tournaments/${id}/edit`}>Редактировать турнир</Link>
            )}
            {isAdmin && (
                <div style={{ marginTop: "20px", borderTop: "1px solid gray", paddingTop: "10px" }}>
                    <h3>Блокировка турнира</h3>
                    <textarea
                        placeholder="Причина блокировки"
                        value={banReason}
                        onChange={(e) => setBanReason(e.target.value)}
                        rows={3}
                        style={{ width: "100%", marginBottom: "10px" }}
                    />
                    <button onClick={handleBanTournament}>Заблокировать турнир</button>
                </div>
            )}
            {canRegister && tournament.solo ? (
                <div>
                    <textarea
                        placeholder="Введите информацию о заявке"
                        value={info}
                        onChange={(e) => setInfo(e.target.value)}
                    />
                    <button onClick={handleRegister}>Записаться на турнир</button>
                </div>
            ) : tournament.solo ? (
                <p style={{ color: "red" }}>{registerError || "Вы не можете подать заявку."}</p>
            ) : (
                <div>
                    <select value={selectedTeamId} onChange={(e) => setSelectedTeamId(e.target.value)}>
                        <option value="">Выберите команду</option>
                        {teams.map(team => (
                            <option key={team.id} value={team.id}>{team.name}</option>
                        ))}
                    </select>
                    <textarea
                        placeholder="Введите информацию о заявке"
                        value={info}
                        onChange={(e) => setInfo(e.target.value)}  // Обновляем состояние информации
                    />
                    <button onClick={handleTeamRegister} disabled={!selectedTeamId}>Подать заявку от команды</button>
                </div>
            )}

            <Link to={`/tournaments/${id}/Entries`}>Заявки</Link>

            <h2>Участники</h2>
            {participants.length === 0 ? (
                <p>Пока нет участников.</p>
            ) : (
                <ul>
                    {participants.map((p) => (
                        <li key={p.id}>
                            {p.userName ? (
                                <Link to={`/profile/${p.userId}`}>{p.userName}</Link>
                            ) : (
                                <Link to={`/team/${p.teamId}`}>{p.teamName}</Link>
                            )}
                        </li>
                    ))}
                </ul>
            )}

            <h2>Календарь матчей</h2>
            {matches.length === 0 ? (
                <p>Матчи пока не назначены.</p>
            ) : (
                <ul>
                    {matches.map((match) => (
                        <li key={match.id}>
                            <Link to={`/matches/${match.id}`}>
                                {match.team1Name} vs {match.team2Name} — {new Date(match.date).toLocaleString()}
                            </Link>
                        </li>
                    ))}
                </ul>
            )}

            <Link to={`/tournaments/${id}/create-match`}><button>Создать матч</button></Link>

            {standings.length > 0 && (
                <>
                    <h2>Турнирная таблица</h2>
                    <table>
                        <thead>
                            <tr>
                                <th>Участник</th>
                                <th>Игры</th>
                                <th>Победы</th>
                                <th>Ничьи</th>
                                <th>Поражения</th>
                                <th>Очки</th>
                            </tr>
                        </thead>
                        <tbody>
                            {standings.map((row, idx) => (
                                <tr key={idx}>
                                    <td>{row.name}</td>
                                    <td>{row.matches}</td>
                                    <td>{row.wins}</td>
                                    <td>{row.draws}</td>
                                    <td>{row.losses}</td>
                                    <td>{row.points}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </>
            )}
            {bracket.length > 0 && (
                <>
                    <h2>Турнирная сетка</h2>
                    <ul>
                        {bracket.map((item, idx) => (
                            <li key={idx}>
                                Матч {item.matchId}: {item.player1 ?? item.team1} vs {item.player2 ?? item.team2}
                            </li>
                        ))}
                    </ul>
                </>
            )}

            {tournament && tournament.format?.name === 'Playoff' && (
                <button onClick={createBracket}>Создать турнирную сетку</button>
            )}

            {userSchedule.length > 0 && (
                <div>
                    <h2>Ваше расписание</h2>
                    <ul>
                        {userSchedule.map(match => (
                            <li key={match.id}>
                                <Link to={`/matches/${match.id}`}>
                                    {match.team1Name} vs {match.team2Name} — {new Date(match.date).toLocaleString()}
                                </Link>
                            </li>
                        ))}
                    </ul>
                </div>
            )}

        </div>
    );
};

export default TournamentDetails;

