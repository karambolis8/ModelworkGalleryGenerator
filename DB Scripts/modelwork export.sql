SELECT FROM_UNIXTIME(topic_time) 'Data utworzenia', FROM_UNIXTIME(topic_last_post_time) 'Data ostatniego posta', CONCAT('http://modelwork.pl/viewtopic.php?f=', forum_id, '&t=', topic_id) 'URL', topic_title 'Tytu³', topic_first_poster_name 'Autor'
FROM phpbb3_topics
WHERE forum_id = 68
ORDER BY topic_time DESC;