namespace Rediska.Tests.Commands.Server
{
    using System.Linq;
    using FluentAssertions;
    using NUnit.Framework;
    using Protocol;
    using Rediska.Commands.Server;

    public class INFO_Should
    {
        private static readonly BulkString allSections = new PlainBulkString(
            @"# Server
redis_version:6.0.5
redis_git_sha1:00000000
redis_git_dirty:0
redis_build_id:af6959b92be038f4
redis_mode:standalone
os:Linux 4.4.0-18362-Microsoft x86_64
arch_bits:64
multiplexing_api:epoll
atomicvar_api:atomic-builtin
gcc_version:9.3.0
process_id:8595
run_id:7bcd3a130ecbfeef7330cc43c4992366c96369e9
tcp_port:6379
uptime_in_seconds:24468
uptime_in_days:0
hz:10
configured_hz:10
lru_clock:16218300
executable:/home/user/redis-6.0.5/src/./redis-server
config_file:

# Clients
connected_clients:1
client_recent_max_input_buffer:2
client_recent_max_output_buffer:0
blocked_clients:0
tracking_clients:0
clients_in_timeout_table:0

# Memory
used_memory:866440
used_memory_human:846.13K
used_memory_rss:2580480
used_memory_rss_human:2.46M
used_memory_peak:885272
used_memory_peak_human:864.52K
used_memory_peak_perc:97.87%
used_memory_overhead:819882
used_memory_startup:802824
used_memory_dataset:46558
used_memory_dataset_perc:73.19%
allocator_allocated:1305504
allocator_active:1589248
allocator_resident:4407296
total_system_memory:34228121600
total_system_memory_human:31.88G
used_memory_lua:37888
used_memory_lua_human:37.00K
used_memory_scripts:0
used_memory_scripts_human:0B
number_of_cached_scripts:0
maxmemory:0
maxmemory_human:0B
maxmemory_policy:noeviction
allocator_frag_ratio:1.22
allocator_frag_bytes:283744
allocator_rss_ratio:2.77
allocator_rss_bytes:2818048
rss_overhead_ratio:0.59
rss_overhead_bytes:-1826816
mem_fragmentation_ratio:3.13
mem_fragmentation_bytes:1756576
mem_not_counted_for_evict:0
mem_replication_backlog:0
mem_clients_slaves:0
mem_clients_normal:16986
mem_aof_buffer:0
mem_allocator:jemalloc-5.1.0
active_defrag_running:0
lazyfree_pending_objects:0

# Persistence
loading:0
rdb_changes_since_last_save:0
rdb_bgsave_in_progress:0
rdb_last_save_time:1593273339
rdb_last_bgsave_status:ok
rdb_last_bgsave_time_sec:3
rdb_current_bgsave_time_sec:-1
rdb_last_cow_size:0
aof_enabled:0
aof_rewrite_in_progress:0
aof_rewrite_scheduled:0
aof_last_rewrite_time_sec:-1
aof_current_rewrite_time_sec:-1
aof_last_bgrewrite_status:ok
aof_last_write_status:ok
aof_last_cow_size:0
module_fork_in_progress:0
module_fork_last_cow_size:0

# Stats
total_connections_received:1
total_commands_processed:17
instantaneous_ops_per_sec:0
total_net_input_bytes:540
total_net_output_bytes:38407
instantaneous_input_kbps:0.00
instantaneous_output_kbps:0.00
rejected_connections:0
sync_full:0
sync_partial_ok:0
sync_partial_err:0
expired_keys:0
expired_stale_perc:0.00
expired_time_cap_reached_count:0
expire_cycle_cpu_milliseconds:333
evicted_keys:0
keyspace_hits:2
keyspace_misses:0
pubsub_channels:0
pubsub_patterns:0
latest_fork_usec:7127
migrate_cached_sockets:0
slave_expires_tracked_keys:0
active_defrag_hits:0
active_defrag_misses:0
active_defrag_key_hits:0
active_defrag_key_misses:0
tracking_total_keys:0
tracking_total_items:0
tracking_total_prefixes:0
unexpected_error_replies:0

# Replication
role:master
connected_slaves:0
master_replid:53045c23ade45f83ef2f2c39651c62a634cf4daf
master_replid2:0000000000000000000000000000000000000000
master_repl_offset:0
second_repl_offset:-1
repl_backlog_active:0
repl_backlog_size:1048576
repl_backlog_first_byte_offset:0
repl_backlog_histlen:0

# CPU
used_cpu_sys:0.078125
used_cpu_user:0.015625
used_cpu_sys_children:0.015625
used_cpu_user_children:0.000000

# Modules

# Commandstats
cmdstat_info:calls=6,usec=387,usec_per_call=64.50
cmdstat_ping:calls=1,usec=2,usec_per_call=2.00
cmdstat_lolwut:calls=1,usec=302,usec_per_call=302.00
cmdstat_geohash:calls=1,usec=6,usec_per_call=6.00
cmdstat_xinfo:calls=1,usec=9,usec_per_call=9.00
cmdstat_geoadd:calls=3,usec=48,usec_per_call=16.00
cmdstat_acl:calls=3,usec=52,usec_per_call=17.33
cmdstat_geopos:calls=1,usec=27,usec_per_call=27.00

# Cluster
cluster_enabled:0

# Keyspace
db0:keys=1,expires=0,avg_ttl=0"
        );

        [Test]
        public void Parse_Sections()
        {
            var sut = new INFO(INFO.AllSections);
            var sections = allSections.Accept(sut.ResponseStructure);
            sections
                .Select(section => section.Name)
                .Should()
                .Equal(
                    "Server",
                    "Clients",
                    "Memory",
                    "Persistence",
                    "Stats",
                    "Replication",
                    "CPU",
                    "Modules",
                    "Commandstats",
                    "Cluster",
                    "Keyspace"
                );
        }
    }
}